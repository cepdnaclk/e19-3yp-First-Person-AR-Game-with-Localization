using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class UDPController : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public string esp8266IP = "192.168.1.231"; // Replace with your ESP8266's IP
    public int esp8266Port = 12345; // Replace with your ESP8266's port
    private int localPort = 12345;

    void Start()
    {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(esp8266IP), esp8266Port);

        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
    }

    public void SendUDPMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, remoteEndPoint);
    }

    private void ReceiveCallback(System.IAsyncResult ar)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, localPort);
        byte[] receivedBytes = udpClient.EndReceive(ar, ref ipEndPoint);
        string receivedMessage = Encoding.ASCII.GetString(receivedBytes);

        Debug.Log("Received message from ESP8266: " + receivedMessage);

        // Restart listening for UDP messages
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
