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
            Debug.Log("facing enemy");
            if (draining == null)
                draining = StartCoroutine(Draining());
            if (filling != null)
            {
                StopCoroutine(filling);
                filling = null;
            }
        }
        else
        {
            Debug.Log("facing away from enemy");
            if (filling == null)
                filling = StartCoroutine(Filling()); 
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
        float angle = Vector3.Angle(cameraTransf.forward, enemyDist); //calculating if camera is facing enemy
        return angle < dirLimit; 
    }

    private IEnumerator Draining()
    {
        while (IsFacingEnemy() && slider != null && slider.value > 0f)
        {
            Debug.Log("bar draining");
            SetHealth((int)slider.value - 1);
            yield return new WaitForSeconds(0.05f);    
        }
        draining = null;
    }
    private IEnumerator Filling() // working on bar fill when looking away from enemy
    {
        while (!IsFacingEnemy() && slider != null && slider.value < slider.maxValue)
        {
            Debug.Log("bar filling");
            SetHealth((int)slider.value + 1);
            yield return new WaitForSeconds(0.05f);
        }
        filling = null;
    }

    public void SetMaxHealth(int health)
    {
        if (slider == null) return;
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        if (slider == null) return;

        int previous = (int)slider.value;
        int clamped = Mathf.Clamp(health, 0, (int)slider.maxValue);
        slider.value = clamped;

        if (clamped == 0 && previous > 0)
        {
            OnHealthZero();
        }
    }

    private void OnHealthZero()
    {
        Debug.Log("health at 0");
        if (jumpscare != null)
            jumpscare.SetActive(true); // trigger jumpscare
        onHealthZero?.Invoke();
    }
}