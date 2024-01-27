using UnityEngine;
using UnityEngine.UI;

public class ARCameraResolveMenu : MonoBehaviour
{
    public Camera arCamera;
    public RawImage rawImage;

    RenderTexture renderTexture;

    void Start()
    {
        // Create or access the RenderTexture with the same dimensions as the screen
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Set the target texture of the AR camera to the RenderTexture
        arCamera.targetTexture = renderTexture;

        // Display the RenderTexture on the RawImage
        rawImage.texture = renderTexture;

    }

    void OnDestroy()
    {
        // Release resources
        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}
