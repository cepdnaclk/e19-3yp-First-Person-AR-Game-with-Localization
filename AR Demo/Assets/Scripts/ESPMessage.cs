using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESPMessage : MonoBehaviour
{

    public UDPController udpController;
    public string messageInput; // If you want user input

    public void OnButtonClick()
    {
        string messageToSend = messageInput; // Get message from input field or set a default message
        udpController.SendUDPMessage(messageToSend);
    }
}


