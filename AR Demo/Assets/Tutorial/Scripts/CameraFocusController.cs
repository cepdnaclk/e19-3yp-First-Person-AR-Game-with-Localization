using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/* Class that sets the focus mode for the AR camera */
public class CameraFocusController : MonoBehaviour
{
    private ARSession arSession;
    private bool arSessionStarted = false;

    void Start()
    {
        arSession = FindObjectOfType<ARSession>();

        if (arSession != null)
            arSession.enabled = true; // Enable the ARSession
    }

    void Update()
    {
        if (arSession != null && arSession.enabled && !arSessionStarted)
        {
            arSessionStarted = true;
            SetAutoFocus();
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            // App resumed
            if (arSessionStarted)
                // App resumed and AR session already started
                // but let's set autofocus again
                SetAutoFocus();
        }
    }

    private void SetAutoFocus()
    {
        ARCameraManager arCameraManager = FindObjectOfType<ARCameraManager>();

        if (arCameraManager != null)
        {
            arCameraManager.focusMode = CameraFocusMode.Auto;
            Debug.Log("Autofocus set");
        }
        else
        {
            Debug.Log("ARCameraManager not found or does not support auto focus");
        }
    }
}
