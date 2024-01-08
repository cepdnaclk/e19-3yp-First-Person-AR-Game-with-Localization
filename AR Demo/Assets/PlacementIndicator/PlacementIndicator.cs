using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private GameObject visual;

    void Start ()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // hide the placement indicator visual
        visual.SetActive(false);
    }

    //void Update ()
    //{
    //    // shoot a raycast from the center of the screen
    //    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //    rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

    //    // if we hit an AR plane surface, update the position and rotation
    //    if(hits.Count > 0)
    //    {
    //        transform.position = hits[0].pose.position;
    //        transform.rotation = hits[0].pose.rotation;

    //        // enable the visual if it's disabled
    //        if(!visual.activeInHierarchy)
    //            visual.SetActive(true);
    //    }
    //}

    void Update()
    {
        // Shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // Find the hit on an AR plane surface
        ARPlane plane = FindFirstDetectedPlane(hits);

        // If a plane is found, update the position and rotation
        if (plane != null)
        {
            transform.position = plane.transform.position;
            transform.rotation = plane.transform.rotation;

            // Enable the visual if it's disabled
            if (!visual.activeInHierarchy)
                visual.SetActive(true);
        }
        else
        {
            // Disable the visual if no plane is detected
            visual.SetActive(false);
        }
    }

    // Function to find the first detected AR plane from the hits
    private ARPlane FindFirstDetectedPlane(List<ARRaycastHit> hits)
    {
        foreach (var hit in hits)
        {
            ARPlane plane = hit.trackable as ARPlane;
            if (plane != null)
            {
                // Found a detected AR plane, return it
                return plane;
            }
        }
        // No AR planes detected
        return null;
    }

}