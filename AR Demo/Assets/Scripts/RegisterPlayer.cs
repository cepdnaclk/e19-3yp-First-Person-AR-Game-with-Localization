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

    IEnumerator Notification(string text)
    {
        textBox.text = text;
        yield return text;
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response code: " + request.responseCode);
            StartCoroutine(Notification("An error occurred: " + request.error));
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    public void Register()
    {
        if (password.text == re_password.text)
        {
            string url = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/signup";

            string jsonData = "{\"email\":\"" + username.text +
                              "\",\"password\":\"" + password.text +
                              "\",\"gunid\":\"" + "gunid" +
                              "\",\"gloveid\":\"" + "gloveid" +
                              "\",\"headsetid\":\"" + "headsetid" + "\"}";

            StartCoroutine(PostRequest(url, jsonData));
        }
        else
        {
            StartCoroutine(Notification("Passwords are not equal"));
        }
    }
}
