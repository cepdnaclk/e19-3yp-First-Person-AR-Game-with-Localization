using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class CameraFeedSenderUDP : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // Replace with your server's IP
    public int serverPort = 12345; // Replace with your server's port

    private WebCamTexture webcamTexture;
    private UdpClient client;
    private IPEndPoint endPoint;
    private bool isSending = false;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            client = new UdpClient();
            endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            isSending = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Socket error: " + e.Message);
        }
    }

    void Update()
    {
        if (isSending)
        {
            if (!webcamTexture.isPlaying)
                return;

            Texture2D texture = new Texture2D(webcamTexture.width, webcamTexture.height);
            texture.SetPixels(webcamTexture.GetPixels());
            byte[] bytes = texture.EncodeToPNG();

            SendFrame(bytes);
        }
    }

    void SendFrame(byte[] frame)
    {
        try
        {
            client.Send(frame, frame.Length, endPoint);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending frame: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        isSending = false;
        if (client != null)
            client.Close();
    }
}
