using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESPObjectMovement : MonoBehaviour
{
    private Vector3 initialPosition; // Store initial reference position

    // Method to set the initial reference position for the ESP8266 object
    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }

    // Method to update the ESP8266 object's position based on received data
    public void UpdateESPObjectPosition(Vector3 espPosition)
    {
        // Calculate the displacement from the initial reference position to ESP8266 position
        Vector3 displacement = espPosition - initialPosition;

        // Apply the displacement to the ESP8266 object's position
        transform.position += displacement;
    }
}
