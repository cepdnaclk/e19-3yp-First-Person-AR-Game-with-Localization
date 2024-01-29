//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Timeline;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;
//[RequireComponent(typeof(ARRaycastManager))]
//public class ESPObjectPlacement : MonoBehaviour
//{
//    public GameObject med_kit, bullets, mark;
//    private float lastHealth; // Store the last health value
//    private int lastScore; // Store the last score value
//    public TMP_Text positionText;// Reference to the position text
//    public GameObject gameObjectToInstatiate;
//    private GameObject spawnedObject;
//    private ARRaycastManager raycastManager;
//    private Vector2 touchPos;
//    public HealthController healthController; // Reference to the HealthController script
//    public Shoot shoot; // Reference to the Shoot script
//    private bool locationConfirmed = false; // Flag to confirm location
//    private Vector3 selectedPosition; // Variable to store the selected position
//    public Canvas canvas; // Reference to the Canvas
//    public GameObject[] elementsToDisable; // Array to hold child elements
//    static List<ARRaycastHit> hitList = new();
//    float currentHealth;
//    int currentScore;

//    // Start is called before the first frame update
//    private void Awake()
//    {
//        raycastManager = GetComponent<ARRaycastManager>();

//    }
//    private void Start()
//    {
//        DisableChildElements();
//        currentHealth = healthController.healthSlider.value;
//        currentScore = shoot.score;
//    }

//    bool TryGetTouchPos(out Vector2 touchPos)
//    {
//        if (Input.touchCount > 0)
//        {
//            touchPos = Input.GetTouch(0).position;
//            return true;
//        }
//        touchPos = default;
//        return false;
//    }


//    void Update()


//    {

//        if (locationConfirmed)
//        {
//            Debug.Log("Location confirmed...");
//            // Logic for displaying objects based on conditions
//            currentHealth = healthController.healthSlider.value;
//            currentScore = shoot.score;

//            if (currentHealth == lastHealth && currentScore - lastScore == 3)
//            {
//                Debug.Log("Displaying bullets...");
//                // Display bullets if health is unchanged and score is incremented by 3
//                spawnedObject = Instantiate(bullets, selectedPosition, Quaternion.identity);

//                // Update last health and score values for the next iteration
//                lastHealth = currentHealth;
//                lastScore = currentScore;
//            }
//            else if ((currentHealth <= lastHealth * 0.8 && currentScore - lastScore == 2) || currentHealth <= lastHealth * 0.6)
//            {
//                Debug.Log("Displaying med kit...");
//                // Display med kit if health has decreased by 20% and score is incremented by 2
//                // Or if health has decreased by 40%
//                spawnedObject = Instantiate(med_kit, selectedPosition, Quaternion.identity);

//                // Update last health and score values for the next iteration
//                lastHealth = currentHealth;
//                lastScore = currentScore;
//            }
//        }
//        if (!TryGetTouchPos(out Vector2 touchPos))

//        {
//            Debug.Log("Touch not detected...");
//            return;
//        }
//        if (raycastManager.Raycast(touchPos, hitList, TrackableType.PlaneWithinPolygon))
//            {
//                var hitPose = hitList[0].pose;
//                if ((spawnedObject == null))
//                {
//                    spawnedObject = Instantiate(gameObjectToInstatiate, hitPose.position, Quaternion.identity);
//                    selectedPosition = hitPose.position; // Set selected position here
//                Debug.Log("Object placed on " + selectedPosition);

//                    // Instantiate mark at the selected position
                    
//                    locationConfirmed = true; // Confirm the location
//                    EnableChildElements(); // Enable child elements
//                }
                

                
//                /*
//                else
//                {
//                    spawnedObject.transform.position = hitPose.position;
//                }
//                */
//            }

        
//    }

//        void DisableChildElements()
//        {
//            Debug.Log("Disabling child elements...");
//            if (canvas != null && elementsToDisable != null)
//            {
//                foreach (GameObject element in elementsToDisable)
//                {
//                    element.SetActive(false);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Canvas or elementsToDisable array is not assigned!");
//            }
//        }
//        void EnableChildElements()
//        {
//            Debug.Log("Enabling child elements...");
//            if (canvas != null && elementsToDisable != null)
//            {
//                foreach (GameObject element in elementsToDisable)
//                {
//                    element.SetActive(true);
//                }
//                positionText.text = "";
//            }
//            else
//            {
//                Debug.LogWarning("Canvas or elementsToDisable array is not assigned!");
//            }
//        }


    
//}
