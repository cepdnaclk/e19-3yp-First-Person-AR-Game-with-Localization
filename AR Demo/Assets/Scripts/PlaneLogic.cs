using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneLogic : MonoBehaviour
{
    [SerializeField] private Toggle Vertical, Horizontal, GamePrepare;
    [SerializeField] private Button startButton;

    private ARPlaneDetect arPlaneDetect;

    public bool VerticalPlaneToggle 
    { 
        get=> Vertical.isOn;
        set
        {
            Vertical.isOn = value;
            checkAll();
        } 
    }
    public bool HorizontalPlaneToggle
    {
        get => Horizontal.isOn;
        set
        {
            Horizontal.isOn = value;
            checkAll();
        }
    }
    public bool GamePrepareToggle
    {
        get => GamePrepare.isOn;
        set
        {
            GamePrepare.isOn = value;
            checkAll();
        }
    }

    private void OnEnable()
    {
        arPlaneDetect = FindAnyObjectByType<ARPlaneDetect>();
        arPlaneDetect.OnVerticalPlaneFound += () => VerticalPlaneToggle = true;
        arPlaneDetect.OnHorizontalPlaneFound += () => HorizontalPlaneToggle = true;
        arPlaneDetect.OnGamePrepare += () => GamePrepareToggle = true;
    }

    private void OnDisable()
    {
        arPlaneDetect.OnVerticalPlaneFound -= () => VerticalPlaneToggle = true;
        arPlaneDetect.OnHorizontalPlaneFound -= () => HorizontalPlaneToggle = true;
        arPlaneDetect.OnGamePrepare -= () => GamePrepareToggle = true;
    }

    private void checkAll()
    {
        if (Vertical && Horizontal && GamePrepare)
        {
            startButton.interactable = true;
        }
    }
}
