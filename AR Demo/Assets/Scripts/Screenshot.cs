using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public string screenshotPrefix = "ARDemo"; 

    public void TakeScreenshot()
    {
        // Generate a filename with a timestamp
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string screenshotName = screenshotPrefix + "_" + timestamp + ".png";

        // Capture a screenshot and save it with the generated filename
        ScreenCapture.CaptureScreenshot(screenshotName);
    }
}
