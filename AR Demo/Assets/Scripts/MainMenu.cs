using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   
    public TMPro.TMP_Text userName;
    
    void Start()
    {
        string displayName = PlayerPrefs.GetString("DisplayName", "");
        Debug.Log("Display Name: " + displayName);
        userName.text = displayName;
    }

    //void Update()
    //{
    //        Debug.Log(userName.text);   
    //}
    
}
