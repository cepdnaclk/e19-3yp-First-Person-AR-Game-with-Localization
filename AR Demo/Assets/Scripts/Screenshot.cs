using System;
using UnityEngine;
using static JoinGame;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;

public class ScreenShotImage
{
    public string img;
    public string email;
}


public class Screenshot : MonoBehaviour
{

    string email;
    string screenshotPrefix = "ARDemo";
    string OpenCVEndpoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/opencv";

    private void Start()
    {
        email = PlayerPrefs.GetString("DisplayName","");
    }

    public void TakeScreenshot()
    {
        // Capture the screen pixels
        Texture2D screenTexture = CaptureScreen();

        // Encode the texture to base64
        string base64String = TextureToBase64(screenTexture);

        // Now, you can use the base64String as needed
        Debug.Log("Base64 Encoded Image:\n" + base64String);

        ScreenShotImage screenShotImage = new ScreenShotImage {
            img = base64String,
            email = email
        };


        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(screenShotImage);

        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(SendImageAndGetToken(OpenCVEndpoint, json));
    }


    IEnumerator SendImageAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest SendImageRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        SendImageRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        SendImageRequest.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        SendImageRequest.SetRequestHeader("Content-Type", "application/json");
        SendImageRequest.SetRequestHeader("auth-token", accessToken);

        // Send login request
        yield return SendImageRequest.SendWebRequest();

        // Check for login errors
        if (SendImageRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Send Image failed");
            //Debug.Log("Request Headers: " + string.Join(", ", SendImageRequest.GetRequestHeader("auth-key")));
            //Debug.Log("Response Code: " + SendImageRequest.responseCode);
            //Debug.Log("Response Text: " + SendImageRequest.downloadHandler.text);
            Debug.LogError("Send Image Error: " + SendImageRequest.error);

        }
        else
        {
            Debug.Log("Success");
            


            
        }
    }

    private Texture2D CaptureScreen()
    {
        // Create a texture and read the screen pixels into it
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        return screenTexture;
    }

    private string TextureToBase64(Texture2D texture)
    {
        try
        {
            byte[] imageBinaryData = texture.EncodeToPNG();
            string base64Encoded = Convert.ToBase64String(imageBinaryData);
            string base64String = Encoding.UTF8.GetString(Encoding.Default.GetBytes(base64Encoded));
            return base64String;
        }
        catch (Exception e)
        {
            Debug.LogError("Error encoding texture to base64: " + e.Message);
            return null;
        }
    }
}
