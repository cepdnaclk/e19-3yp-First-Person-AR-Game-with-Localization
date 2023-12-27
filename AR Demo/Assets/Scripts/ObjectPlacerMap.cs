using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacerMap : MonoBehaviour
{
    public GameObject barrelPrefab;
    public GameObject sandbagPrefab;
    public GameObject vehiclePrefab;
    public GameObject wallPrefab;

    public void SpawnBarrelClone()
    {
        InstantiateObject(barrelPrefab);
    }

    public void SpawnSandbagClone()
    {
        InstantiateObject(sandbagPrefab);
    }

    public void SpawnVehicleClone()
    {
        InstantiateObject(vehiclePrefab);
    }

    public void SpawnWallClone()
    {
        InstantiateObject(wallPrefab);
    }

    void InstantiateObject(GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            // You might want to position the clone relative to the user in AR
            // Example: clone.transform.position = CalculatePositionRelativeToUser();
        }
        else
        {
            Debug.LogWarning("Prefab is not assigned.");
        }
    }

    // Example method to calculate position relative to the user in AR
    Vector3 CalculatePositionRelativeToUser()
    {
        // Implement logic to calculate position relative to the user in AR
        // For AR Foundation, you might use raycasting from the camera to a plane
        // and position the object where the ray intersects with the plane.
        return Vector3.zero; // Change this to the calculated position
    }
}
