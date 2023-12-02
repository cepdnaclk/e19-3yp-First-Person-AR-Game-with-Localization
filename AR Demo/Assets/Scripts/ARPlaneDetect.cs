using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneDetect : MonoBehaviour
{

    private ARPlaneManager m_planeManager;
    private List<ARPlane> m_planeList;

    public event Action OnVerticalPlaneFound;
    public event Action OnHorizontalPlaneFound;
    public event Action OnGamePrepare;

    private bool Vertical = false, Horizontal = false;

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
        if(args.added != null && args.added.Count > 0)
        {
            m_planeList.AddRange(args.added);
        }

        foreach(ARPlane plane in m_planeList.Where(plane=>plane.extents.x*plane.extents.y>=0.1f)) {
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

            if(Vertical && Horizontal)
            {
                OnGamePrepare.Invoke();
            }
            
        }
    }

}
