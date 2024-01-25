using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions.Samples.PersistentCloudAnchors;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player {
    public string email;
    public string gameid;
    
}

public class JoinGame : MonoBehaviour

{
    public TMP_InputField gameIDInputField;
    string email;
    private string accessToken;
    string gameID;
    string JoinGameEndpoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/joingame";
    public string cloudID;

    public delegate void ApiCallback(string response);

    private ResolveMenuManager resolveMenuManager;

    // Callback function variable
    public ApiCallback onApiCallback;




    // Start is called before the first frame update
    void Start()
    {
        email = PlayerPrefs.GetString("DisplayName", "");
        accessToken = PlayerPrefs.GetString("AccessToken", "");
        gameID = gameIDInputField.text;
    }

    public void joinGame()
    {
        string playeremail = email;
        // Create an instance of the CloudData class and set its properties
        Player cloudData = new Player
        {
            email = playeremail,
            gameid = gameIDInputField.text
    };

        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(cloudData);

        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(JoinGameAndGetToken(JoinGameEndpoint, json));
    }

    IEnumerator JoinGameAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest JoinGameRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        JoinGameRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        JoinGameRequest.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        JoinGameRequest.SetRequestHeader("Content-Type", "application/json");
        JoinGameRequest.SetRequestHeader("auth-token", accessToken);

        // Send login request
        yield return JoinGameRequest.SendWebRequest();

        // Check for login errors
        if (JoinGameRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Join Game failed");
            //Debug.Log("Request Headers: " + string.Join(", ", JoinGameRequest.GetRequestHeader("auth-key")));
            //Debug.Log("Response Code: " + JoinGameRequest.responseCode);
            //Debug.Log("Response Text: " + JoinGameRequest.downloadHandler.text);
            Debug.LogError("Join Game Error: " + JoinGameRequest.error);

        }
        else
        {
            Debug.Log("Success");
            string responseText = JoinGameRequest.downloadHandler.text;
            // Call the callback function with the API response
            cloudID = setCloudID(responseText);
            Debug.Log("CloudID " + cloudID);
            Debug.Log("API Response: " + responseText);
            PlayerPrefs.SetString("RetreivedID",responseText);
            

            if (onApiCallback != null)
            {
                onApiCallback(responseText);
            }
            SceneManager.LoadScene(7);
        }
    }
    
    public string setCloudID(string cloudID) 
    {
        
            return cloudID;
        
    }

}


