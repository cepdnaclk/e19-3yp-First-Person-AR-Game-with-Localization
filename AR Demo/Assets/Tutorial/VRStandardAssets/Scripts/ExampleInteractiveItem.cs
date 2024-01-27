using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class ExampleInteractiveItem : MonoBehaviour
    {
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private Renderer m_Renderer;
        [SerializeField] private Material m_OverMaterial;
        [SerializeField] private Material m_NormalMaterial;
        [SerializeField] private Material m_ClickedMaterial;

        private void Awake()
        {
            m_NormalMaterial = m_Renderer.material;
        }


        private void OnEnable()
        {
            Debug.Log("Enable");
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
        }

        private void OnDisable()
        {
            Debug.Log("Disable");
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
        }

        //Handle the Over event
        private void HandleOver()
        {
            m_Renderer.material = m_OverMaterial;
            Debug.Log("Show over state");
        }

        //Handle the Out event
        private void HandleOut()
        {
            m_Renderer.material = m_NormalMaterial;
            Debug.Log("Show out state");
        }

        //Handle the Click event
        private void HandleClick()
        {
            m_Renderer.material = m_ClickedMaterial;
            Debug.Log("Show click state");
        }
    }
}