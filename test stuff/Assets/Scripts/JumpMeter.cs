using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;

public class JumpMeter : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public PostProcessVolume settings; // assign all in inspector
    public GameObject jumpScare;
    bool monsterColliding = false;
    public float dirLimit = 15f;
    public AudioSource breathing;
    public AudioSource monsterS;

    [Header("Health Settings")]
    public int CurrentHealth = 100;
    public int MaxHealth = 100;
    public int MinHealth = 0;
    public UnityEvent onHealthZero;

    private Coroutine dying;
    private Coroutine regen;
    private Coroutine breath;

    public void Start() // making sure vignette is assigned
    {
        if (settings == null)
        {
            Debug.LogWarning("vignette not assigned");
        }
    }

    #region Enemy Detection

    private void Update() // checking each frame if player is facing enemy
    {
        if (monsterColliding == true && settings != null)
        {
            if (dying == null && breath == null)
                dying = StartCoroutine(HealthLoss());
                breath = StartCoroutine(Breathing());

            if (regen != null)
            {
                StopCoroutine(regen);
                regen = null;
            }
        }
        else
        {
            if (regen == null)
                regen = StartCoroutine(Filling()); 

            if (dying != null && breath != null)
            {
                StopCoroutine(dying);
                StopCoroutine(breath);
                dying = null;
                breath = null;
            }
        }
    }

    public void OnCollisionEnter(Collision collide) // checking if monster collided 
    {
        if (collide.gameObject.name == "monster")
        {
            monsterColliding = true;
            monsterS.PlayOneShot(monsterS.clip);
            Debug.Log("monster colliding");
        }
    }

    public void OnCollisionExit(Collision collide) // checking if monster stopped colliding
    {
        if (collide.gameObject.name == "monster")
        {
            monsterColliding = false;
            Debug.Log("monster not colliding");
        }
    }

    #endregion

    #region Health Stuff
    private IEnumerator HealthLoss() // draining health when facing enemy
    {
        while (monsterColliding == true && settings != null)
        {
            ApplyDamage(10);
            if (CurrentHealth <= MinHealth) break;
            Debug.Log("health draining");
            yield return new WaitForSeconds(0.001f);
        }
        dying = null;
    }

    private IEnumerator Filling() // bar fill when looking away from enemy
    {
        while (monsterColliding == false && settings != null)
        {
            ApplyRegen(1);
            if (CurrentHealth >= MaxHealth) break;
            Debug.Log("health filling");
            yield return new WaitForSeconds(1f);
        }
        regen = null;
    }

    private IEnumerator Breathing() // breathing sound when low health
    {
        while (CurrentHealth <= 60)
        {
            if (breathing != null && !breathing.isPlaying)
            {
                breathing.Play();
                breathing.volume = 1f - ((float)CurrentHealth / MaxHealth);
            }
            yield return null;
        }
        if (breathing != null && breathing.isPlaying)
        {
            breathing.Stop();
        }
        breath = null;
    }

    private void ApplyDamage(int amount) // applying changes to health
    {
        SetHealth(CurrentHealth - amount);
    }

    private void ApplyRegen(int amount)
    {
        SetHealth(CurrentHealth + amount);
    }

    public void SetMaxHealth(int health) // setting health and checking for zero health
    {
        MaxHealth = Mathf.Max(1, health);
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);
        UpdateVignette();
    }

    public void SetHealth(int health)
    {
        int previous = CurrentHealth;
        CurrentHealth = Mathf.Clamp(health, MinHealth, MaxHealth);

        UpdateVignette();

        if (CurrentHealth <= 5 && previous > 0)
        {
            OnHealthZero();
        }
    }

    #endregion

    #region Vignette + health at zero
    private void UpdateVignette() // updating vignette intensity based on health
    {
        if (settings == null) return;

        settings.profile.TryGetSettings(out Vignette vignette);
        settings.profile.TryGetSettings(out Grain grain);

        grain.intensity.value = 1.5f - ((float)CurrentHealth / MaxHealth);
        grain.size.value = 1.25f - ((float)CurrentHealth / MaxHealth);
        vignette.intensity.value = 1.05f - ((float)CurrentHealth / MaxHealth);

    }

    #endregion

    private void OnHealthZero() // triggering jumpscare on zero health
    {
        Debug.Log("health at 0");
        if (jumpScare != null)
            jumpScare.SetActive(true);
        onHealthZero?.Invoke();

        settings.profile.TryGetSettings(out Vignette vignette);
        settings.profile.TryGetSettings(out Grain grain);

        grain.intensity.value = 1f;
        grain.size.value = 1f;
        vignette.intensity.value = 1f;
        breathing.volume = 0f;

    }
}