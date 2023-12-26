using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[Serializable]
public class AnchorData
{
    public string anchorId;
    public Vector3 position;
    public Quaternion rotation;

    public AnchorData(ARAnchor anchor)
    {
        anchorId = anchor.trackableId.ToString();
        position = anchor.transform.position;
        rotation = anchor.transform.rotation;
    }
}

