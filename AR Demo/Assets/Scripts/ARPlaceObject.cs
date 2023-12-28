using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlaceObject : MonoBehaviour
{
    private GameObject gameObjectToInstantiate;

    //Four prefabs
    public GameObject barrel, barrier, vehicle, sandbag;
    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private Vector2 touchPos;

    
    public Camera arCamera;


    static List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    private Dictionary<GameObject, List<GameObject>> spawnedObjects = new Dictionary<GameObject, List<GameObject>>();

    private bool objectPlaced = false;

    private bool canPlace = true;
    private float cooldownTime = 2.0f; // Adjust this cooldown time as needed
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

        if (raycastManager.Raycast(touchPos, hitList, TrackableType.PlaneWithinPolygon) && gameObjectToInstantiate != null && canPlace)
        {
            var hitPose = hitList[0].pose;


            GameObject newObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
            if (spawnedObjects.ContainsKey(gameObjectToInstantiate))
            {
                spawnedObjects[gameObjectToInstantiate].Add(newObject);
            }
            else
            {
                List<GameObject> newList = new List<GameObject>();
                newList.Add(newObject);
                spawnedObjects.Add(gameObjectToInstantiate, newList);
            }
            canPlace = false;
        }
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
    }

    public void ClicktoPlaceBarrier()
    {
        gameObjectToInstantiate = barrier;
        Debug.Log("Barrier");
    }

    public void ClicktoPlaceVehicle()
    {
        gameObjectToInstantiate = vehicle;
        Debug.Log("Vehicle");
    }   

    public void ClicktoPlaceSandbag()
    {
        gameObjectToInstantiate = sandbag;
        Debug.Log("Sandbag");
    }

}