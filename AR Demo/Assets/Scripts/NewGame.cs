using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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

    public void AddPlayer()
    {
        string playerID = inputField.text;
        // Create login data in JSON format
        string addPlayerJson = "{\"email\":\"" + userName.text +
                              "\",\"users\":\"" + playerID + "\"}";

        // Get a reference to the Button component
        StartCoroutine(AddPlayerAndGetToken(AddPlayerEndpoint, addPlayerJson));
        // Debug.Log($"{username.text}:{password.text}");
    }
    


    IEnumerator AddPlayerAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest AddPlayerRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        AddPlayerRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        AddPlayerRequest.downloadHandler = new DownloadHandlerBuffer();
        AddPlayerRequest.SetRequestHeader("Content-Type", "application/json");

        // Send login request
        yield return AddPlayerRequest.SendWebRequest();

        // Check for login errors
        if (AddPlayerRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Login failed");
            Debug.LogError("Login Error: " + AddPlayerRequest.error);
        }
        else
        {
            Debug.Log("Success");
        }
    }
}
