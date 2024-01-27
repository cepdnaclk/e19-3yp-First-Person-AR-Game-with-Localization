using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;

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

    string getQrEndPoint = "https://z760hx70mc.execute-api.ap-southeast-1.amazonaws.com/beta/getqr";

public void getQr()
    {
        // string email = Email.text;
        string email = "e19236@eng.pdn.ac.lk";
        // Create an instance of the PlayerData class and set its properties
        Data data = new Data
        {
            email = email,
        };

        // Convert the PlayerData object to a JSON-formatted string
        string json = JsonUtility.ToJson(data);

        // Output the generated JSON string
        Debug.Log(json);
        StartCoroutine(getQrAPI(getQrEndPoint, json));
    }


    IEnumerator getQrAPI(string url, string jsonData)
    {
        // Create a UnityWebRequest for login
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        string accessToken = PlayerPrefs.GetString("AccessToken");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("auth-token", accessToken);

        // Send login request
        yield return request.SendWebRequest();

        // Check for login errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Add Player failed");
            //Debug.Log("Request Headers: " + string.Join(", ", request.GetRequestHeader("auth-key")));
            //Debug.Log("Response Code: " + request.responseCode);
            //Debug.Log("Response Text: " + request.downloadHandler.text);
            Debug.LogError("Add Player Error: " + request.error);

        }
        else
        {
            // Extract the presigned URL from the response
            string jsonResponse = request.downloadHandler.text;
            string presignedUrl = JsonUtility.FromJson<ResponseData>(jsonResponse).presigned_url;

            Debug.Log(presignedUrl);

            // Start a new UnityWebRequest to download the file from the presigned URL
            UnityWebRequest downloadRequest = UnityWebRequest.Get(presignedUrl);

            // Send the download request
            yield return downloadRequest.SendWebRequest();

            // Check for errors
            if (downloadRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("File download failed: " + downloadRequest.error);
            }
            else
            {
                // Save the downloaded file
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads/qr_code.png";
                    File.WriteAllBytes(filePath, downloadRequest.downloadHandler.data);
                    Debug.Log("File saved to: " + filePath);
            }
        }
    }
}

