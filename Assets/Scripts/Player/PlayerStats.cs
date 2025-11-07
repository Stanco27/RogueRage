using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField]
    private float baseMoveSpeed = 5f;

    [SerializeField]
    private float damageMultiplier = 1f;

    [SerializeField]
    private float baseMaxHealth = 100f;

    [Header("Live Stats")]
    public float currentMoveSpeed;
    public float currentDamage;

    public float currentMaxHealth;
    public float currentHealth;

    public int killCount = 0;

    [Header("Currency & Experience")]
    public float currentCurrency = 0.00f;
    public float currentExperience = 0.00f;
    private float expMultiplier = 1.0f;
    private float currencyMultiplier = 1.0f;
    public float collectionRadius = 1.0f;

    public UnityEvent<float> OnCurrencyChanged;
    public UnityEvent<float> OnExperienceChanged;
    public UnityEvent<int> OnKillsChanged;

    void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
        currentDamage = damageMultiplier;

        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
    }

    public void AddCurrency(int amount)
    {
        float totalAmount = amount * currencyMultiplier;
        currentCurrency += totalAmount;
        OnCurrencyChanged?.Invoke(currentCurrency);
        Debug.Log($"Gained {totalAmount} Gold! Total Gold: {currentCurrency}");
    }

    public void AddExperience(int amount)
    {
        float totalAmount = amount * expMultiplier;
        currentExperience += totalAmount;
        Debug.Log($"Gained {totalAmount} EXP! Total EXP: {currentExperience}");
    }

    public void AddKill()
    {
        killCount += 1;
        OnKillsChanged?.Invoke(killCount);
        Debug.Log($"Total Kills: {killCount}");
    }
}
