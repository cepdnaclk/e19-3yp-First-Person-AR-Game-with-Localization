using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneLogic : MonoBehaviour
{
    [SerializeField] private Toggle Vertical, Horizontal, GamePrepare;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject uiPanel; // Reference to the UI panel

    private ARPlaneDetect arPlaneDetect;
    private ARPlaneManager planeManager;
    private ARTaptoPlace arTapToPlace;

    private bool verticalFound = false;
    private bool horizontalFound = false;

    private void Start()
    {
        arPlaneDetect = FindAnyObjectByType<ARPlaneDetect>();
        arPlaneDetect.OnVerticalPlaneFound += () => { VerticalPlaneToggle = true; };
        arPlaneDetect.OnHorizontalPlaneFound += () => { HorizontalPlaneToggle = true; };
        arPlaneDetect.OnGamePrepare += () => { GamePrepareToggle = true; };

        planeManager = FindObjectOfType<ARPlaneManager>();
        arTapToPlace = FindObjectOfType<ARTaptoPlace>();
        arTapToPlace.enabled = false;
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        uiPanel.SetActive(false); // Hide the UI panel

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        arTapToPlace.enabled = true;
        startButton.interactable = false;
    }

    public bool VerticalPlaneToggle
    {
        get => Vertical.isOn;
        set
        {
            Vertical.isOn = value;
            verticalFound = value;
            CheckAllPlanesFound();
        }
    }

    public bool HorizontalPlaneToggle
    {
        get => Horizontal.isOn;
        set
        {
            Horizontal.isOn = value;
            horizontalFound = value;
            CheckAllPlanesFound();
        }
    }

    public bool GamePrepareToggle
    {
        get => GamePrepare.isOn;
        set
        {
            GamePrepare.isOn = value;
            CheckAllPlanesFound();
        }
    }

    private void CheckAllPlanesFound()
    {
        if (verticalFound && horizontalFound && GamePrepare.isOn)
        {
            startButton.interactable = true;
        }
    }
}
