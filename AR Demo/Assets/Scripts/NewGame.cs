using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[System.Serializable]
public class PlayerData
{
    public string email;
    public List<string> users;
    public List<string> stationid;
}

public class NewGame : MonoBehaviour
{

    public TMP_Text userName;
    public TMP_InputField inputField;
    string AddPlayerEndpoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/newgame";
    // Start is called before the first frame update
    void Start()
    {
        string Name = PlayerPrefs.GetString("DisplayName", "");
        userName.text = Name;
    }

    List<string> users = new List<string>();
    List<string> stationid = new List<string>();

    public void AddPlayer()
    {
        users.Add(inputField.text);
        Debug.Log(users.Count);
    }
    
    public void sendPlayers()
    {
        string email = userName.text;
        // Create an instance of the PlayerData class and set its properties
        PlayerData playerData = new PlayerData
        {
            email = email,
            users = users,
            stationid = stationid
        };

        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(playerData);

        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(AddPlayerAndGetToken(AddPlayerEndpoint, json));
    }


    IEnumerator AddPlayerAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest AddPlayerRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        AddPlayerRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        AddPlayerRequest.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        AddPlayerRequest.SetRequestHeader("Content-Type", "application/json");
        AddPlayerRequest.SetRequestHeader("auth-token", accessToken);

        // Send login request
        yield return AddPlayerRequest.SendWebRequest();

        // Check for login errors
        if (AddPlayerRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Add Player failed");
            //Debug.Log("Request Headers: " + string.Join(", ", AddPlayerRequest.GetRequestHeader("auth-key")));
            //Debug.Log("Response Code: " + AddPlayerRequest.responseCode);
            //Debug.Log("Response Text: " + AddPlayerRequest.downloadHandler.text);
            Debug.LogError("Add Player Error: " + AddPlayerRequest.error);

        }
        else
        {
            Debug.Log("Success");
        }
    }
}
