using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{

    
    public GameObject image;

    public AudioSource audioSource;
    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
           if (other.CompareTag("Player"))
        {
            // Enable GameObject
            if (image != null)
                image.SetActive(true);

            // Play sound
            if (audioSource != null && sound != null)
                audioSource.PlayOneShot(sound);
        }
    }
}
