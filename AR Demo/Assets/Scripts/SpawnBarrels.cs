using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrels : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] barrels;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(barrels[i], spawnPoints[i].position, Quaternion.identity);
        }
        
    }

 

    // Update is called once per frame
    void Update()
    {
        
    }
}
