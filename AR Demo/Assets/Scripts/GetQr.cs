using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
using TMPro;

[System.Serializable]
public class Data
{
    public string email;
}

[System.Serializable]
public class ResponseData
{
    public string presigned_url;
}

public class GetQr : MonoBehaviour
{
    public TMP_Text Email;
    public TMP_Text Notification;

    string getQrEndPoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/getqr";

    public void getQr()
    {
        Notification.text = "Welcome to ARCombat";
        string email = Email.text;
        Data data = new Data
        {
            email = email,
        };

        string json = JsonUtility.ToJson(data);
        StartCoroutine(getQrAPI(getQrEndPoint, json));
    }

    IEnumerator getQrAPI(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("auth-token", accessToken);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request failed: " + request.error);
            Notification.text = "Request Failed";
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonResponse);
            string presignedUrl = responseData.presigned_url;

            // Open the presigned URL in the device's default web browser
            Application.OpenURL(presignedUrl);
            Notification.text = "File downloading...";
        }
    }
}
