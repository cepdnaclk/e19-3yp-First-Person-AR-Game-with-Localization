using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions.Samples.PersistentCloudAnchors;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlaceObject : MonoBehaviour
{

    public ARViewManager aRViewManager;
    private GameObject gameObjectToInstantiate;
    

    //Four prefabs
    public GameObject barrel, barrier, vehicle, sandbag;

    private ARRaycastManager raycastManager;
    
    public Camera arCamera;


    static List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    private Dictionary<GameObject, List<GameObject>> spawnedObjects = new Dictionary<GameObject, List<GameObject>>();


    private bool canPlace = true;
    private readonly float cooldownTime = 5.0f; // Adjust this cooldown time as needed
    private float cooldownTimer;

    public TMPro.TextMeshProUGUI moveButtonText;
    public TMPro.TextMeshProUGUI deleteButtonText;


    private bool canMove = false;
    private bool canDelete = false;
    private Vector2 touchPosition;
    private PlacementObject lastSelectedObject;

    // Start is called before the first frame update
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        cooldownTimer = cooldownTime;

    }

    bool TryGetTouchPos(out Vector2 touchPos)
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }
        touchPos = default;
        return false;
    }

    
    void Update()
    {
        if(canMove|| canDelete)
        {
            return;
        }
        
        if (!canPlace)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canPlace = true;
                cooldownTimer = cooldownTime;
            }
        }

        if (!TryGetTouchPos(out Vector2 touchPos))
        {
            return;
        }

        //if (raycastManager.Raycast(touchPos, hitList, TrackableType.PlaneWithinPolygon) && gameObjectToInstantiate != null && canPlace)
        //{
        //    var hitPose = hitList[0].pose;


        //    GameObject newObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
        //    if (spawnedObjects.ContainsKey(gameObjectToInstantiate))
        //    {
        //        spawnedObjects[gameObjectToInstantiate].Add(newObject);
        //    }
        //    else
        //    {
        //        List<GameObject> newList = new List<GameObject>();
        //        newList.Add(newObject);
        //        spawnedObjects.Add(gameObjectToInstantiate, newList);
        //    }
        //    Debug.Log("Object placed");
        //    //GetAnchorPoint(newObject);
        //    canPlace = false;
        //}
    }

    void CheckObjectManipulation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (canMove)
                    {
                        // Move the object to the touched position
                        hit.transform.position = hit.point;
                    }
                    else if (canDelete)
                    {
                        if (hit.collider.gameObject.tag == "barrel(Clone)")
                        {
                            Destroy(hit.transform.gameObject);
                            
                        }

                    }
                }
            }
        }
    }


    public void ToggleMove()
    {
        canPlace = false;
        canMove = !canMove;
        canDelete = false;
        if (canMove)
        {
            moveButtonText.text = "Done"; // Update button text
            deleteButtonText.text = "Delete";
        }
        else
        {
            moveButtonText.text = "Move"; // Update button text
        }
    }

    public void ToggleDelete()
    {
        canPlace = false;
        canDelete = !canDelete;
        canMove = false;

        if (canDelete)
        {
            deleteButtonText.text = "Done"; // Update button text
            moveButtonText.text = "Move";
        }
        else
        {
            deleteButtonText.text = "Delete"; // Update button text
        }
    }
    public void SetObjectToInstantiate(GameObject objectPrefab)
    {
        gameObjectToInstantiate = objectPrefab;
    }

    public void ClicktoPlaceBarrel()
    {
        gameObjectToInstantiate = barrel;
        Debug.Log("Barrel");
        aRViewManager.CloudAnchorPrefab = gameObjectToInstantiate;
    }

    public void ClicktoPlaceBarrier()
    {
        gameObjectToInstantiate = barrier;
        Debug.Log("Barrier");
        aRViewManager.CloudAnchorPrefab = gameObjectToInstantiate;
    }

    public void ClicktoPlaceVehicle()
    {
        gameObjectToInstantiate = vehicle;
        Debug.Log("Vehicle");
        aRViewManager.CloudAnchorPrefab = gameObjectToInstantiate;
    }   

    public void ClicktoPlaceSandbag()
    {
        gameObjectToInstantiate = sandbag;
        Debug.Log("Sandbag");
        aRViewManager.CloudAnchorPrefab = gameObjectToInstantiate;
    }

    //public void GetAnchorPoint(GameObject placedObject)
    //{
    //    if (placedObject != null)
    //    {

    //        if (placedObject.TryGetComponent<ARAnchor>(out var anchor))
    //        {
    //            Vector3 anchorPosition = anchor.transform.position;
    //            Quaternion anchorRotation = anchor.transform.rotation;

    //            Debug.Log("Anchor Position: " + anchorPosition);
    //            Debug.Log("Anchor Rotation: " + anchorRotation.eulerAngles);
    //        }
    //        else
    //        {
    //            Debug.Log("Object does not have an ARAnchor component.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("No object placed.");
    //    }
    //}

    
    //public void AnchorPlacedObjects()
    //{
    //    foreach (var entry in spawnedObjects)
    //    {
    //        List<GameObject> objectsList = entry.Value;
    //        foreach (GameObject obj in objectsList)
    //        {
    //            ActivateAnchor(obj);
    //            AnchorObject(obj);
    //        }
    //    }
    //}

    //[System.Obsolete]
    //void AnchorObject(GameObject obj)
    //{
    //    if (obj != null)
    //    {
    //        ARAnchor currentAnchor = obj.GetComponent<ARAnchor>();
    //        ARAnchorManager anchorManager = FindObjectOfType<ARAnchorManager>(); // Get the ARAnchorManager

            

    //        if (currentAnchor != null && anchorManager != null)
    //        {
    //            anchorManager.RemoveAnchor(currentAnchor); // Remove the existing anchor
    //        }

    //        ARAnchor newAnchor = obj.AddComponent<ARAnchor>();
    //        if (newAnchor != null)
    //        {
    //            newAnchor.transform.position = obj.transform.position;
    //            newAnchor.transform.rotation = obj.transform.rotation;
    //            Debug.Log("Object anchored: " + obj.name);
    //        }
    //    }
    //}



    //void ActivateAnchor(GameObject obj)
    //{
    //    ARAnchor anchor = obj.GetComponent<ARAnchor>();

    //    if (anchor != null)
    //    {
    //        anchor.enabled = true; // Activate the ARAnchor
    //    }
    //}
}