using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, interacted;

    public AudioSource s;
    public int winAmount = 10; // number of items needed to win

    private bool insideTrigger;
    private static int itemCount = 0; // track collected items

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && insideTrigger)
        {
            s.Play();
            interacted?.Invoke();
            exitedTrigger.Invoke();
            insideTrigger = false;
            itemCount++;
            MonsterSpawn2.updateSpeed(itemCount); // notify spawner
        }

        if (itemCount >= winAmount)
        {
            // trigger win condition
            SceneManager.LoadScene("EndScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enteredTrigger.Invoke();
            insideTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exitedTrigger.Invoke();
            insideTrigger = false;
        }
    }
}
