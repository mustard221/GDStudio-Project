using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class JumpMeter : MonoBehaviour
{
    public Slider slider; // assign each in inspector
    public GameObject jumpscare;
    public Transform cameraTransf;
    public Transform enemyTransf;  
    public float dirLimit = 15f;
    public UnityEvent onHealthZero;

    private Coroutine draining;

    private void Update()
    {
        if (IsFacingEnemy())
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

    private bool IsFacingEnemy()
    {
        if (cameraTransf == null || enemyTransf == null)
            return false;

        if (!enemyTransf.gameObject.activeInHierarchy)
            return false;

        Vector3 enemyDist = (enemyTransf.position - cameraTransf.position).normalized;
        float angle = Vector3.Angle(cameraTransf.forward, enemyDist);
        return angle < dirLimit; 
    }

    private IEnumerator Draining()
    {
        while (IsFacingEnemy() && slider.value > 0)
        {
            Debug.Log("looking at enemy");
            SetHealth((int)slider.value - 1);
            yield return new WaitForSeconds(0.1f);    
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
        if (slider.value > 0 && health <= 0)
        {
            slider.value = 0;
            OnHealthZero();
        }
        else
        {
            slider.value = health;
        }
    }

    private void OnHealthZero()
    {
        Debug.Log("health at 0");
        jumpscare.SetActive(true); // trigger jumpscare
    }
}