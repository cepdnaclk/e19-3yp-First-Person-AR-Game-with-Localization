using System;
using UnityEngine;
using static JoinGame;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using System.IO;

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
        Debug.Log(email);
    }

    public void TakeScreenshot()
    {
        // Capture the screen pixels
        Texture2D screenTexture = CaptureScreen();
        // Generate a filename with a timestamp
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string screenshotName = screenshotPrefix + "_" + timestamp + ".png";

        // Capture a screenshot and save it with the generated filename
        ScreenCapture.CaptureScreenshot(screenshotName);

        // Encode the texture to base64
        string base64String = TextureToBase64(screenTexture);

        // Now, you can use the base64String as needed
        Debug.Log("Base64 Encoded Image:\n" + base64String);

        ScreenShotImage screenShotImage = new ScreenShotImage {
            img = base64String,
            email = email
        };
        Debug.Log(screenShotImage.img);
        Debug.Log(screenShotImage.email);


        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(screenShotImage);
        json = ReadJsonFromFile();
        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(SendImageAndGetToken(OpenCVEndpoint, json));
    }


    IEnumerator SendImageAndGetToken(string url, string jsonData)
    {
        Debug.Log(url);
        jsonData = ReadJsonFromFile();
        // Create a UnityWebRequest for login
        UnityWebRequest SendImageRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        SendImageRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        SendImageRequest.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        SendImageRequest.SetRequestHeader("Content-Type", "application/json");
        SendImageRequest.SetRequestHeader("auth-token", accessToken);
        Debug.Log("Request Headers: " + string.Join(", ", SendImageRequest.GetRequestHeader("auth-token")));

        // Send login request
        yield return SendImageRequest.SendWebRequest();

        // Check for errors
        if (SendImageRequest.result != UnityWebRequest.Result.Success && SendImageRequest.responseCode == 404)
        {
            string responseText = SendImageRequest.downloadHandler.text;
            Debug.Log("API Response: " + responseText);
        }
        else if (SendImageRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Protocol Error. Response Code: " + SendImageRequest.responseCode);
            Debug.LogError("Response Text: " + SendImageRequest.downloadHandler.text);
            Debug.LogError("Error: " + SendImageRequest.error);
        }
        else
        {
            Debug.Log("Success");

            string responseText = SendImageRequest.downloadHandler.text;
            Debug.Log("API Response: " + responseText);


        }
    }

    void SaveJsonToFile(string jsonData)
    {
        // Save JSON data to a file (you can modify the file path as needed)
        string filePath = Application.persistentDataPath + "/image_data.json";
        File.WriteAllText(filePath, jsonData);
        Debug.Log("JSON data saved to file: " + filePath);
    }

    string ReadJsonFromFile()
    {
        // Specify the file path (modify as needed)
        string filePath = "D:\\Academic\\Semester 5\\3YP\\e19-3yp-First-Person-AR-Game-with-Localization\\backend\\serverless\\testing\\opencv.json";

        // Check if the file exists
        if (File.Exists(filePath))
        {
            try
            {
                // Read the JSON content from the file
                string jsonData = File.ReadAllText(filePath);
                Debug.Log("JSON data read from file: " + filePath);
                return jsonData;
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading JSON file: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }

        // Return null if there was an issue or the file doesn't exist
        return null;
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
