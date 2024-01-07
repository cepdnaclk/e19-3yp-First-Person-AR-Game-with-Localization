using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{

    private GameObject arObjectToSpawn;
    public GameObject placementIndicator;

    public GameObject barrel, barrier, vehicle, sandbag;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private Dictionary<GameObject, List<GameObject>> spawnedObjects = new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // need to update placement indicator, placement pose and spawn 
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }


        UpdatePlacementPose();
        UpdatePlacementIndicator();


    }
    void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(GetSelectedObjectToSpawn(), PlacementPose.position, PlacementPose.rotation);

        if (spawnedObjects.ContainsKey(spawnedObject))
        {
            spawnedObjects[spawnedObject].Add(spawnedObject);
        }
        else
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(spawnedObject);
            spawnedObjects.Add(spawnedObject, newList);
        }
    }

    GameObject GetSelectedObjectToSpawn()
    {
        if (spawnedObject == barrel)
        {
            return barrel;
        }
        else if (spawnedObject == barrier)
        {
            return barrier;
        }
        else if (spawnedObject == vehicle)
        {
            return vehicle;
        }
        else if (spawnedObject == sandbag)
        {
            return sandbag;
        }
        return null;
    }



    public void ClicktoPlaceBarrel()
    {
        arObjectToSpawn = barrel;
        Debug.Log("Barrel");
    }

    public void ClicktoPlaceBarrier()
    {
        arObjectToSpawn = barrier;
        Debug.Log("Barrier");
    }

    public void ClicktoPlaceVehicle()
    {
        arObjectToSpawn = vehicle;
        Debug.Log("Vehicle");
    }

    public void ClicktoPlaceSandbag()
    {
        arObjectToSpawn = sandbag;
        Debug.Log("Sandbag");
    }


}


