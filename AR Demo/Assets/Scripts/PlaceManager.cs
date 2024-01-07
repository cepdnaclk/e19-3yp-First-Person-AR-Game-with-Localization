using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    private PlacementIndicator placementIndicator;
    public GameObject objectToPlace;
    private GameObject newPlacedObject;
    // Start is called before the first frame update
    void Start()
    {
        placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    public void PlaceObject()
    {
        newPlacedObject = Instantiate(objectToPlace, placementIndicator.transform.position, placementIndicator.transform.rotation);
    }
}
