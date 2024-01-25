using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class VRModeSwitcher : MonoBehaviour
{
    private bool isVRModeEnabled;

    void Start()
    {
        isVRModeEnabled = false;
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Camera.main.ResetAspect();
    }


    public void ToggleVRMode()
    {
        isVRModeEnabled = !isVRModeEnabled;

        if (isVRModeEnabled)
        {
            // Start XR display subsystem
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        else
        {
            // Stop XR display subsystem
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            Camera.main.ResetAspect();
        }

    }
}
