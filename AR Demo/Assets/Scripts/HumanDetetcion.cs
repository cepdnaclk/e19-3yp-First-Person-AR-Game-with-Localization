using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;

public class HumanDetetcion : MonoBehaviour
{
    private CascadeClassifier cascadeClassifier;
    WebCamTexture webCamTexture;

    void Start()
    {
        // Load the pre-trained Haar cascade for full body detection
        cascadeClassifier = new CascadeClassifier();
        cascadeClassifier.load(Application.dataPath + "OpenCVForUnity/Utils/haarcascade_fullbody.xml");
        Debug.Log(Application.dataPath + "/Utils/haarcascade_fullbody.xml");
        // Make sure to have the XML file in your project

        if (cascadeClassifier.empty())
        {
            Debug.LogError("Failed to load cascade classifier.");
            return;
        }

        // Start the webcam or video capture
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
    }

    void Update()
    {
        // Check if the webcam texture is playing
        if (!webCamTexture.isPlaying)
            return;

        // Convert the WebCamTexture to a Mat format
        Mat rgbaMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        Utils.webCamTextureToMat(webCamTexture, rgbaMat);

        // Detect human bodies using the cascade classifier
        MatOfRect bodies = new MatOfRect();
        if (cascadeClassifier != null)
        {
            cascadeClassifier.detectMultiScale(rgbaMat, bodies, 1.1, 2, 2, new Size(30, 30));
        }

        // Draw rectangles around the detected bodies
        foreach (OpenCVForUnity.CoreModule.Rect rect in bodies.toArray())
        {
            Imgproc.rectangle(rgbaMat, new Point(rect.x, rect.y), new Point(rect.x + rect.width, rect.y + rect.height), new Scalar(255, 0, 0), 2);
        }

        // Convert the Mat format back to a Texture2D to display in Unity
        Texture2D texture = new Texture2D(rgbaMat.cols(), rgbaMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(rgbaMat, texture);

        // Display the texture in a RawImage or on a GameObject's material
        // Example: GetComponent<RawImage>().texture = texture;

        // Release resources
        rgbaMat.release();
    }

    void OnDestroy()
    {
        // Release resources when the script is destroyed
        if (cascadeClassifier != null)
        {
            cascadeClassifier.Dispose();
            cascadeClassifier = null;
        }
    }
}
