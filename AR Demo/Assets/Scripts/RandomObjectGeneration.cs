using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class RandomObjectGeneration : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    
    

    public GameObject gameObjectToInstantiate;
    private ARRaycastManager raycastManager;


    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    private GameObject placedObject;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager= GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
    }

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null && placedObject == null)
        {
            ARPlane aRPlane = args.added[0];
            placedObject = Instantiate(gameObjectToInstantiate, aRPlane.transform.position, Quaternion.identity);
          
        }
    }
    void Start()
    {
        
    }

  
    
}
