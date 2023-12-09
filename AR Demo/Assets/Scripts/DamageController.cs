using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private float bombDamage = 10.0f;
    [SerializeField] private HealthController healthController = null;
    [SerializeField] private AudioClip bombAudio;
    private bool playingAudio = false;
    private AudioSource damageAudioSource;

    private void Start()
    {
        damageAudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
