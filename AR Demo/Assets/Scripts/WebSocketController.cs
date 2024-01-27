using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using WebSocketSharp;
using System.Security.Authentication;

public class WebSocketController : MonoBehaviour
{
    private WebSocket ws;
    public TMP_Text scoreText;
    public Slider healthBar;
    public Button shootButton;
    public short score = 0;
    void Start()
    {
        string emailAddress = PlayerPrefs.GetString("DisplayName","");
        string webSocketUrl = $"wss://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/?email={Uri.EscapeDataString(emailAddress)}";
        Debug.Log("Connecting to WebSocket server at " + webSocketUrl);
        ws = new WebSocket(webSocketUrl);

        ws.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;

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

    private void Update()
    {
        
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
            Debug.Log("Hit");
            DecreaseHealth();
        }
        else if (message == "score")
        {
            // Player scored, update score accordingly
            Debug.Log("Score");
            score++;
            scoreText.text = "Score: " + (score);
            
            Debug.Log("Score: " + score);
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
        Debug.Log("Method called");
        score++;
        scoreText.text = "Score: " + (score);
        Debug.Log("Score: " + score);
    }
}
