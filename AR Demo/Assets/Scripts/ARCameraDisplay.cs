using UnityEngine;
using UnityEngine.UI;

public class ARCameraDisplay : MonoBehaviour
{
    public Camera arCamera;
    public RawImage rawImage1;
    public RawImage rawImage2;

    RenderTexture renderTexture;

    void Start()
    {
        // Calculate the square dimensions for the RenderTexture
        int size = Mathf.Min(Screen.width, Screen.height);
        
        // Create or access the RenderTexture with square dimensions
        renderTexture = new RenderTexture(size, size, 24);

        // Set the target texture of the AR camera to the RenderTexture
        arCamera.targetTexture = renderTexture;

        // Display the RenderTexture on the RawImages
        rawImage1.texture = renderTexture;
        rawImage2.texture = renderTexture;

        // Calculate the viewport rect for a square ratio
        Rect viewportRect = new Rect(0f, 0f, 1f, 1f);

        // Apply the viewport rect to the AR camera
        arCamera.rect = viewportRect;
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
