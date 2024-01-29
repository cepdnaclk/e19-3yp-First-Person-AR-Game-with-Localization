using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shoot : MonoBehaviour
{

    public GameObject arCamera;
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;
    
    
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
                scoreText.text = "Score: " + score;
            }
        }
    }
    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
