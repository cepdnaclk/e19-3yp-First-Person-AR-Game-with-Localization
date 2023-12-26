using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;




[System.Serializable]
public class SerializablePlane
{
    public Pose pose;
    public PlaneAlignment alignment; // Optional, depending on your need
    public Vector2 size;

    public SerializablePlane(ARPlane plane)
    {
        pose = plane.transform.GetWorldPose();
        alignment = plane.alignment; // Keep this line if preserving alignment
        size = plane.size;
    }

    public static implicit operator ARPlane(SerializablePlane v)
    {
        throw new NotImplementedException();
    }
}

public class ARPlaneDetect : MonoBehaviour
{

    private ARPlaneManager m_planeManager;
    private List<ARPlane> m_planeList;

    

    public event Action OnVerticalPlaneFound;
    public event Action OnHorizontalPlaneFound;
    public event Action OnGamePrepare;

    private bool Vertical = false, Horizontal = false;


    private void Start()
    {
        LoadPlanes();
    }
    private void OnEnable()
    {

        m_planeList = new List<ARPlane>();
        m_planeManager = FindObjectOfType<ARPlaneManager>();
        m_planeManager.planesChanged += OnPlanesChanged;

    }

    private void OnDisable()
    {

        m_planeManager.planesChanged -= OnPlanesChanged;

    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null && args.added.Count > 0)
        {
            m_planeList.AddRange(args.added);
        }

        foreach (ARPlane plane in m_planeList.Where(plane => plane.extents.x * plane.extents.y >= 0.1f))
        {
            if (plane.alignment.IsVertical())
            {
                OnVerticalPlaneFound.Invoke();
                Vertical = true;
            }
            else
            {
                OnHorizontalPlaneFound.Invoke();
                Horizontal = true;
            }

            if (Horizontal)
            {
                OnGamePrepare.Invoke();
            }

        }
    }
    public void SavePlanes()
    {
        var serializablePlanes = m_planeList.Select(plane => new SerializablePlane(plane)).ToList();
        var json = JsonUtility.ToJson(serializablePlanes);

        // Save planes.json in the project's Assets folder
        string planesFilePath = Path.Combine(Application.dataPath, "planes.json");
        File.WriteAllText(planesFilePath, json);

        // Save data.json in the persistent data path for runtime access
        string dataFilePath = Path.Combine(Application.persistentDataPath, "data.json");
        File.WriteAllText(dataFilePath, json);
    }


    public void LoadPlanes()
    {
        if (System.IO.File.Exists("planes.json"))
        {
            var json = System.IO.File.ReadAllText("planes.json");
            var serializablePlanes = JsonUtility.FromJson<List<SerializablePlane>>(json);

            // Create ARPlane objects from serializablePlanes
            foreach (SerializablePlane serializablePlane in serializablePlanes)
        {
            ARPlane plane = serializablePlane;
            // Add the plane to the ARPlaneManager or your scene
        }
        }
    }

}