using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TokenResponse
{
    public string accessToken;
    public int expires_in; // You can remove this if not needed
}


public class LoginScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text textBox;

    IEnumerator Notification(String text){
        textBox.text = text;
        yield return text;
    }

    IEnumerator LoginAndGetToken(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest loginRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        loginRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        loginRequest.downloadHandler = new DownloadHandlerBuffer();
        loginRequest.SetRequestHeader("Content-Type", "application/json");

        // Send login request
        yield return loginRequest.SendWebRequest();

        // Check for login errors
        if (loginRequest.result != UnityWebRequest.Result.Success)
        {
            StartCoroutine(Notification("Login failed"));
            Debug.LogError("Login Error: " + loginRequest.error);
        }
        else
        {
            // Login successful, extract accessToken from response
            string jsonResponse = loginRequest.downloadHandler.text;
            // Parse the JSON response to get the accessToken
            // (Assuming the response contains a 'accessToken' field)
            string accessToken = ParseTokenFromResponse(jsonResponse);

            // Save the access token to PlayerPrefs
            PlayerPrefs.SetString("AccessToken", accessToken);

            Debug.Log("accessToken: " + PlayerPrefs.GetString("AccessToken"));
            
            SceneManager.LoadScene(2); // main menu
        }
    }

    string ParseTokenFromResponse(string jsonResponse)
    {
        // Deserialize the JSON response into TokenResponse object
        TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(jsonResponse);
        
        // Return the accessToken from the deserialized object
        return tokenResponse.accessToken;
    }

    public void OnLoginClick()
    {
        string loginEndpoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/login";
        // Create login data in JSON format
        string loginJson =  "{\"email\":\"" + username.text +
                              "\",\"password\":\"" + password.text + "\"}";

        // Get a reference to the Button component
        StartCoroutine(LoginAndGetToken(loginEndpoint,loginJson));
        // Debug.Log($"{username.text}:{password.text}");
    }
}
