using UnityEngine;
using UnityEngine.UI;

public class DirectCamera : MonoBehaviour
{
    public RawImage rawImage1;
    public RawImage rawImage2;
    private WebCamTexture webCamTexture;

    void Start()
    {
        // Check if the device has a camera
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("No camera detected on this device.");
            return;
        }

        // Get the default camera
        WebCamDevice defaultCamera = WebCamTexture.devices[0];
        // Create a new WebCamTexture with default camera
        webCamTexture = new WebCamTexture(defaultCamera.name, Screen.width, Screen.height, 30);
        // Play the WebCamTexture
        webCamTexture.Play();

        // Assign the WebCamTexture to the RawImage
        rawImage1.texture = webCamTexture;
        rawImage2.texture = webCamTexture;
    }

    void OnDestroy()
    {
        // Stop the WebCamTexture when the script is destroyed
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}
