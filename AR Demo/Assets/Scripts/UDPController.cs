using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPController : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public string esp8266IP = "192.168.1.231"; // Replace with your ESP8266's IP
    public int esp8266Port = 12345; // Replace with your ESP8266's port

    void Start()
    {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(esp8266IP), esp8266Port);
    }

    public void SendUDPMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, remoteEndPoint);
    }
}
