using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class TokenResponse
{
    public string token;
    public int expires_in; // You can remove this if not needed
}


public class LoginScript : MonoBehaviour
{
    public string loginEndpoint = "https://your-backend-endpoint-url.com/api/login";
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text textBox;

    IEnumerator Notification(String text){
        textBox.text = text;
        yield return text;
    }

    IEnumerator LoginAndGetToken()
    {
        // Create login data in JSON format
        string loginJson = "{\"username\":\"" + username.text + "\",\"password\":\"" + password.text + "\"}";

        // Create a UnityWebRequest for login
        UnityWebRequest loginRequest = new UnityWebRequest(loginEndpoint, "POST");
        loginRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(loginJson);
        loginRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        loginRequest.downloadHandler = new DownloadHandlerBuffer();

        // Send login request
        yield return loginRequest.SendWebRequest();

        // Check for login errors
        if (loginRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Login Error: " + loginRequest.error);
            StartCoroutine(Notification("Login failed"));
        }
        else
        {
            // Login successful, extract token from response
            string jsonResponse = loginRequest.downloadHandler.text;
            // Parse the JSON response to get the token
            // (Assuming the response contains a 'token' field)
            string token = ParseTokenFromResponse(jsonResponse);
            Debug.Log("Token: " + token);
            
            // Here, you might save the token for further use, like storing it in PlayerPrefs or using it in subsequent requests.
        }
    }

    string ParseTokenFromResponse(string jsonResponse)
    {
        // Deserialize the JSON response into TokenResponse object
        TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(jsonResponse);
        
        // Return the token from the deserialized object
        return tokenResponse.token;
    }

    public void OnLoginClick()
    {
        // Get a reference to the Button component
        StartCoroutine(LoginAndGetToken());
        // Debug.Log($"{username.text}:{password.text}");
    }
}
