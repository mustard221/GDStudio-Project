using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    public Slider slider; // assign each in inspector
    public Transform cameraTransf;
    public Transform enemyTransf;  
    public float dirLimit = 15f;

    private Coroutine draining;

    private void Update()
    {
        if (IsFacingEnemy()) // slider lowers only when looking at monster
        {
            if (draining == null)
                draining = StartCoroutine(Draining());
        }
        else
        {
            if (draining != null)
            {
                StopCoroutine(draining);
                draining = null;
            }
        }
    }

    private bool IsFacingEnemy() // detect if looking at active monster
    {
        if (cameraTransf == null || enemyTransf == null)
            return false;

        if (!enemyTransf.gameObject.activeInHierarchy)
            return false;

        Vector3 enemyDist = (enemyTransf.position - cameraTransf.position).normalized;
        float angle = Vector3.Angle(cameraTransf.forward, enemyDist);
        return angle < dirLimit; 
    }

    private IEnumerator Draining() // lowering slider value; -1 per second when looking at monster
    {
        while (IsFacingEnemy() && slider.value > 0)
        {
            Debug.Log("looking at enemy");
            SetHealth((int)slider.value - 1);
            yield return new WaitForSeconds(1f);    
        }
        draining = null;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
