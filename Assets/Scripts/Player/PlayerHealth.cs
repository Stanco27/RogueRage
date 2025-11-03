using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // =======================================================================
    // REFERENCES & EVENTS
    // =======================================================================

    [Header("References")]
    public PlayerStats playerStats;

    public UnityEvent OnDamaged;
    public UnityEvent OnHealed;
    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<Vector3> OnTakenDamage;

    // =======================================================================
    // SCREEN FLASH FEEDBACK
    // =======================================================================

    [Header("Screen Flash Feedback")]
    public Image damageFlashImage;
    public float flashMaxAlpha = 0.5f;
    public float flashDuration = 0.15f;

    private Coroutine screenFlashCoroutine;

    // =======================================================================
    // UNITY LIFECYCLE
    // =======================================================================

    void Awake()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }

        if (damageFlashImage == null)
        {
            GameObject flashObject = GameObject.FindWithTag("DamageFlashUI");
            if (flashObject != null)
            {
                damageFlashImage = flashObject.GetComponent<Image>();
                if (damageFlashImage == null)
                {
                    Debug.LogError(
                        "Found DamageFlashUI object, but it's missing the Image component!",
                        flashObject
                    );
                }
            }
            else
            {
                Debug.LogWarning(
                    "DamageFlashOverlay not found in scene. Screen flash will not work."
                );
            }
        }

        if (playerStats != null)
        {
            OnHealthChanged?.Invoke(playerStats.currentHealth / playerStats.currentMaxHealth);
        }
    }

    // =======================================================================
    // DAMAGE & HEALING LOGIC
    // =======================================================================

    public void TakeDamage(float damageAmount, Vector3 damageSourcePosition)
    {
        if (playerStats == null || playerStats.currentHealth <= 0)
            return;

        playerStats.currentHealth = Mathf.Max(playerStats.currentHealth - damageAmount, 0f);

        OnDamaged?.Invoke();
        OnHealthChanged?.Invoke(playerStats.currentHealth / playerStats.currentMaxHealth);
        OnTakenDamage?.Invoke(damageSourcePosition);

        if (damageFlashImage != null)
        {
            if (screenFlashCoroutine != null)
                StopCoroutine(screenFlashCoroutine);
            screenFlashCoroutine = StartCoroutine(FlashScreenRoutine());
        }

        if (playerStats.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (playerStats == null || playerStats.currentHealth <= 0)
            return;

        playerStats.currentHealth += healAmount;
        playerStats.currentHealth = Mathf.Min(
            playerStats.currentHealth,
            playerStats.currentMaxHealth
        );

        OnHealed?.Invoke();
        OnHealthChanged?.Invoke(playerStats.currentHealth / playerStats.currentMaxHealth);
    }

    // =======================================================================
    // UTILITY & VISUALS
    // =======================================================================

    private IEnumerator FlashScreenRoutine()
    {
        Color flashColor = damageFlashImage.color;

        damageFlashImage.gameObject.SetActive(true);
        damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, flashMaxAlpha);

        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(flashMaxAlpha, 0f, timer / flashDuration);
            damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, newAlpha);
            yield return null;
        }

        damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        damageFlashImage.gameObject.SetActive(false);
        screenFlashCoroutine = null;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " Player has died!");
        OnDeath?.Invoke();
        Destroy(gameObject, 0.1f);
    }

    public float GetNormalizedHealth()
    {
        if (playerStats == null || playerStats.currentMaxHealth <= 0)
            return 0f;
        return playerStats.currentHealth / playerStats.currentMaxHealth;
    }
}
