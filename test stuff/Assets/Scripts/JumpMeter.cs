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
    private Coroutine filling;

    private void Update()
    {
        if (IsFacingEnemy())
        {
            if (draining == null)
            draining = StartCoroutine(Draining());
            StopCoroutine(Filling());
        }
        else
        {
            if (draining != null)
            {
                StopCoroutine(Draining());
                draining = null;
                StartCoroutine(Filling());
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
        float angle = Vector3.Angle(cameraTransf.forward, enemyDist); //calculating if camera is facing enemy
        return angle < dirLimit; 
    }

    private IEnumerator Draining()
    {
        while (IsFacingEnemy() && slider.value > 0)
        {
            Debug.Log("looking at enemy");
            SetHealth((int)slider.value - 1);
            yield return new WaitForSeconds(0.05f);    
        }
        draining = null;
    }
    private IEnumerator Filling() // working on bar fill when looking away from enemy
    {
        while (!IsFacingEnemy() && slider.value < 100)
        {
            Debug.Log("looking away from enemy");
            SetHealth((int)slider.value + 1);
            yield return new WaitForSeconds(0.05f);
        }
        filling = null;
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