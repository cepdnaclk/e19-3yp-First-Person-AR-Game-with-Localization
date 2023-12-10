using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public GameObject arCamera;
    private int score = 0;
    
    
    
    // Start is called before the first frame update
    
    public void ShootBullet()
    {
        RaycastHit hit;

        if(Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
        {
            if (hit.transform.name == "barrel(Clone)")
            {
                Destroy(hit.transform.gameObject);
                score++;
            }
        }
    }
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
