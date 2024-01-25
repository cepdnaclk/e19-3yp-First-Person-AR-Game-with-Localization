using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[System.Serializable]
public class CloudData
{
    public string email;
    public string anchorid;
}


public class CloudAnchor : MonoBehaviour
{
    string cloidAnchorId;
    string gameID;
    string accessToken;
    string CloudAnchorEndpoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/arcore";

    public void sendCloudIdtoDatabase(string cloudID)
    {
        string email = gameID;
        // Create an instance of the CloudData class and set its properties
        CloudData cloudData = new CloudData
        {
            email = email,
            anchorid = cloudID
        };

        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(cloudData);

        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(AddCLoudAnchorAndGetToken(CloudAnchorEndpoint, json));
    }

    void Start()
    {
       gameID = PlayerPrefs.GetString("DisplayName", "");
        accessToken = PlayerPrefs.GetString("AccessToken", "");
    }



    IEnumerator AddCLoudAnchorAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest AddCloudIDRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        AddCloudIDRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        AddCloudIDRequest.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        AddCloudIDRequest.SetRequestHeader("Content-Type", "application/json");
        AddCloudIDRequest.SetRequestHeader("auth-token", accessToken);

        // Send login request
        yield return AddCloudIDRequest.SendWebRequest();

        // Check for login errors
        if (AddCloudIDRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Add CloudID failed");
            //Debug.Log("Request Headers: " + string.Join(", ", AddCloudIDRequest.GetRequestHeader("auth-key")));
            //Debug.Log("Response Code: " + AddCloudIDRequest.responseCode);
            //Debug.Log("Response Text: " + AddCloudIDRequest.downloadHandler.text);
            Debug.LogError("Add CLoudID Error: " + AddCloudIDRequest.error);

        }
        else
        {
            Debug.Log("Success");
        }
    }
}
