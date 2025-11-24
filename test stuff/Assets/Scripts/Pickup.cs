using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, interacted;

    private bool insideTrigger;
    private static int itemCount = 0; // track collected items

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && insideTrigger)
        {
            interacted?.Invoke();
            exitedTrigger.Invoke();
            insideTrigger = false;
            itemCount++;
            MonsterSpawn2.updateSpeed(itemCount); // notify spawner
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
