using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using WebSocketSharp;

public class WebSocketController : MonoBehaviour
{
    private WebSocket ws;
    public TMP_Text scoreText;
    public Slider healthBar;
    public Button shootButton;

    void Start()
    {
        string webSocketUrl = "wss://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/";
        ws = new WebSocket(webSocketUrl);

        // Set up WebSocket event handlers
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection opened.");
        };

        ws.OnMessage += (sender, e) =>
        {
            // Handle incoming messages from the server
            HandleWebSocketMessage(e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket error: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket connection closed.");
        };

        // Connect to the WebSocket server
        ws.Connect();

        // Subscribe to button click event
        shootButton.onClick.AddListener(OnShootButtonClick);
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }

    void OnShootButtonClick()
    {
        // Send a message to the WebSocket server when the button is clicked
        ws.Send("ShootButtonClicked");
    }

    void HandleWebSocketMessage(string message)
    {
        // Handle incoming messages from the server
        // Modify this function based on your server's message format
        Debug.Log("Received message from server: " + message);

        // Example: Assuming server sends messages like "Hit", "Score", or null
        if (message == "hit")
        {
            // Player hit, update health accordingly
            DecreaseHealth();
        }
        else if (message == "score")
        {
            // Player scored, update score accordingly
            IncreaseScore();
        }
        else if (message == "null")
        {
            // Do nothing for null response
        }
        else
        {
            // Handle other response cases as needed
        }
    }

    void DecreaseHealth()
    {
        // Update your health bar UI here (example: decrease health by 5)
        healthBar.value -= 5;
    }

    void IncreaseScore()
    {
        // Update your score text UI here (example: increase score by 1)
        int currentScore = int.Parse(scoreText.text.Split(':')[1].Trim());
        scoreText.text = "Score: " + (currentScore + 1).ToString();
    }
}
