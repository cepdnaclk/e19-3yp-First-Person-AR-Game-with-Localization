using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PhonePoseTracker : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;

        if (arCamera == null)
        {
            Debug.LogError("ARCamera not found. Ensure it's tagged as MainCamera.");
            return;
        }
    }

    void Update()
    {
        if (arCamera != null)
        {
            // Get the ARCamera's position and rotation
            Vector3 cameraPosition = arCamera.transform.position;
            Quaternion cameraRotation = arCamera.transform.rotation;

            // Use the camera position and rotation as needed
            // For example, interact with objects based on camera position or rotation

            // Print position and rotation values to the console
            Debug.Log("ARCamera Position: " + cameraPosition);
            Debug.Log("ARCamera Rotation: " + cameraRotation.eulerAngles);
        }
    }
}
