using System;
using UnityEngine;

using UnityEngine.UI;
using WebSocketSharp;
using System.Security.Authentication;

public class WebSocketController : MonoBehaviour
{
    private WebSocket ws;
    public Text scoreText, scoreText2;
    public Slider healthBar, healthBar2;
    public Button shootButton, shootButton2;
    public short score = 0;
    public float health = 1.0f;


    void Start()
    {
        string emailAddress = PlayerPrefs.GetString("DisplayName","");
        
        string webSocketUrl = $"wss://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/?email={Uri.EscapeDataString(emailAddress)}";
        Debug.Log("Connecting to WebSocket server at " + webSocketUrl);
        ws = new WebSocket(webSocketUrl);

        ws.SslConfiguration.EnabledSslProtocols =
    SslProtocols.Tls |
        SslProtocols.Tls11 |
            SslProtocols.Tls12;

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
        
        healthBar2.value = health;
        healthBar.value = health;
        scoreText.text = "Score: " + (score);
        scoreText2.text = "Score: " + (score);

        Debug.Log("Score2 : " + scoreText2.text);
        Debug.Log("health2 : " + healthBar2.value);
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
            IncreaseScore();
        }
        
        else
        {
            // Handle other response cases as needed
        }
    }

    void DecreaseHealth()
    {
        // Update your health bar UI here (example: decrease health by 5)
        health -= 0.05f;
        Debug.Log("Health: " + health);

    }

    void IncreaseScore()
    {
        // Update your score text UI here (example: increase score by 1)
        Debug.Log("Method called");
        score++;
        
        Debug.Log("Score: " + score);
        
        

        
    }
}
