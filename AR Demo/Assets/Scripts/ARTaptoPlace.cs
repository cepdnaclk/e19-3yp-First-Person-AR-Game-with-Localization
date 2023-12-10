using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTaptoPlace : MonoBehaviour
{
    public GameObject gameObjectToInstatiate;
    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private Vector2 touchPos;

    static List<ARRaycastHit> hitList = new List<ARRaycastHit>();


    // Start is called before the first frame update
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

    }

    bool TryGetTouchPos(out Vector2 touchPos)
    {
        if(Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }
        touchPos = default; 
        return false;
    }


    void Update()
    {
      if(!TryGetTouchPos(out Vector2 touchPos))
        {
            return;
        }
        if (raycastManager.Raycast(touchPos, hitList, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hitList[0].pose;
            if ((spawnedObject == null)){
                spawnedObject = Instantiate(gameObjectToInstatiate, hitPose.position, hitPose.rotation);
            }
            /*
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
            */
        }
    }
}
