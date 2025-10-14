using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, interacted;

    private bool insideTrigger;
    private static int collectedItemCount = 0; // track collected items

    void Update()
    {
        if (insideTrigger && Input.GetKeyDown(KeyCode.E))
        {
            interacted?.Invoke();
            exitedTrigger.Invoke();
            insideTrigger = false;

            collectedItemCount++;
            MonsterSpawn.UpdateSpawnInterval(collectedItemCount); // notify spawner
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
