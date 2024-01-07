using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System;

public class RegisterPlayer : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField re_password;
    [SerializeField] private TMP_Text textBox;

    IEnumerator Notification(String text){
        textBox.text = text;
        yield return text;
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Convert JSON data to byte array
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Set the request body
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            StartCoroutine(Notification("An error occured"));
        }
        else
        {
            // Request successful, process the response
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    public void Register()
    {
        Debug.Log(password,re_password);
        if (password.text == re_password.text) {
            string url = "https://your-backend-endpoint-url.com/api/endpoint";
            
            // Example JSON data to send in the request
            string jsonData = "{\"username\":\"" + username.text + "\",\"password\":\"" + password.text + "\"}";

            StartCoroutine(PostRequest(url, jsonData));
        } else {
            Debug.Log("Passwords are not equal");
            StartCoroutine(Notification("Passwords are not equal"));
        }
    }
}
