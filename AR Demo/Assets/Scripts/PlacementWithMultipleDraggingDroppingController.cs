using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithMultipleDraggingDroppingController : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    public GameObject barrel, barrier, sandbags, vehicle;



    [SerializeField]
    private Camera arCamera;

    private PlacementObject[] placedObjects;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlacementObject lastSelectedObject;

    [SerializeField]
    private Button Sandbags, Barrels, Barrier,Vehicle;

    private GameObject PlacedPrefab 
    {
        get 
        {
            return placedPrefab;
        }
        set 
        {
            placedPrefab = value;
        }
    }


    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();


        if(Sandbags != null && Barrels != null && Barrier != null && Vehicle != null) 
        {
            Sandbags.onClick.AddListener(() => ChangePrefabSelection(sandbags));
            Barrels.onClick.AddListener(() => ChangePrefabSelection(barrel));
            Barrier.onClick.AddListener(() => ChangePrefabSelection(barrier));
            Vehicle.onClick.AddListener(() => ChangePrefabSelection(vehicle));
        }
    }

    private void ChangePrefabSelection(GameObject gameObject)
    {
        GameObject loadedGameObject = gameObject;
        if(loadedGameObject != null)
        {
            PlacedPrefab = loadedGameObject;
            Debug.Log($"Game object with name {gameObject.name} was loaded");
        }
        else 
        {
            Debug.Log($"Unable to find a game object with name {gameObject.name}");
        }
    }

    

    void Update()
    {
        

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    lastSelectedObject = hitObject.transform.GetComponent<PlacementObject>();
                    if(lastSelectedObject != null)
                    {
                        PlacementObject[] allOtherObjects = FindObjectsOfType<PlacementObject>();
                        foreach(PlacementObject placementObject in allOtherObjects)
                        {
                            placementObject.Selected = placementObject == lastSelectedObject;
                        }
                    }
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                lastSelectedObject.Selected = false;
            }
        }

        if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if(lastSelectedObject == null)
            {
                lastSelectedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
            }
            else 
            {
                if(lastSelectedObject.Selected)
                {
                    lastSelectedObject.transform.position = hitPose.position;
                    lastSelectedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }


}
