using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectGenerator : MonoBehaviour
{
    public ARSessionOrigin arSessionOrigin;
    public GameObject arObjectPrefab; // The AR object you want to place

    void Start()
    {
        if (arSessionOrigin != null && arObjectPrefab != null)
        {
            // Generate the AR object at position (1, 1, 1) and rotation (0, 0, 0)
            Vector3 position = new Vector3(0f, 200f, 0f);
            Quaternion rotation = Quaternion.identity;

            Instantiate(arObjectPrefab, position, rotation, arSessionOrigin.transform);
        }
        else
        {
            Debug.LogError("ARSessionOrigin or AR Object Prefab not assigned.");
        }
    }
}
