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

    void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
        currentDamage = damageMultiplier;

        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
    }
}
