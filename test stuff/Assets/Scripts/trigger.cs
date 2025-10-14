using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public AudioSource audioSource;
    private float normalvolume;
    private float lowerVolume = 0.2f;

    void Start()
    {
        normalvolume = audioSource.volume; 
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.volume = lowerVolume;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.volume = normalvolume;
        }
    }
}
