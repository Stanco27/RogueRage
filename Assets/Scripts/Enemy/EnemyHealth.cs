using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;

    public UnityEvent OnDamaged;
    public UnityEvent OnDeath;

    [Header("Visual Feedback")]
    public Renderer targetRenderer;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.1f;
    public float deathShrinkDuration = 0.5f;

    private Color originalColor;
    private Coroutine flashCoroutine;
    private Coroutine shrinkCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;

        if (targetRenderer != null && targetRenderer.material != null)
        {
            originalColor = targetRenderer.material.color;
        }
    }

    public void TakeDamage(float damageAmount, Vector3 damageSourcePosition)
    {
        if (currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);

        OnDamaged?.Invoke();

        if (targetRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FlashColorRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashColorRoutine()
    {
        targetRenderer.material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        targetRenderer.material.color = originalColor;
        flashCoroutine = null;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        OnDeath?.Invoke();

        if (targetRenderer != null)
        {
            if (shrinkCoroutine != null)
                StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = StartCoroutine(ShrinkAndDestroyRoutine());
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private IEnumerator ShrinkAndDestroyRoutine()
    {
        Vector3 initialScale = transform.localScale;
        float timer = 0f;

        while (timer < deathShrinkDuration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(
                initialScale,
                Vector3.zero,
                timer / deathShrinkDuration
            );
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
