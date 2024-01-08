using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeployLogDisplayTMP : MonoBehaviour
{
    public TMP_Text debugLogText;
    public float clearInterval = 5f; // Time interval to clear text

    private float timer = 0f;

    void Start()
    {
        Application.logMessageReceived += HandleLog;
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if the interval has passed
        if (timer >= clearInterval)
        {
            ClearText();
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        debugLogText.text += logString + "\n";
    }

    void ClearText()
    {
        debugLogText.text = ""; // Clear the text
        timer = 0f; // Reset the timer
    }
}
