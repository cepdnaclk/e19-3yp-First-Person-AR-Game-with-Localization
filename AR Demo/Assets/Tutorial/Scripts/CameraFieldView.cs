using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFieldView : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        this.GetComponent<Camera>().fieldOfView = GameObject.Find("ARCamera").GetComponent<Camera>().fieldOfView;
    }
}
