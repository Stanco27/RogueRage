using UnityEngine;

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

    [Header("Currency & Experience")]
    public float currentCurrency = 0.00f;
    public float currentExperience = 0.00f;
    private float expMultiplier = 1.0f;
    private float currencyMultiplier = 1.0f;
    public float collectionRadius = 1.0f;

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
        Debug.Log($"Gained {totalAmount} Gold! Total Gold: {currentCurrency}");
    }

    public void AddExperience(int amount)
    {
        float totalAmount = amount * expMultiplier;
        currentExperience += totalAmount;
        Debug.Log($"Gained {totalAmount} EXP! Total EXP: {currentExperience}");
    }
}
