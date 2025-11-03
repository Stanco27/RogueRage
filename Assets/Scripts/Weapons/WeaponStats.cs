using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField]
    private float baseDamage = 10f;

    [SerializeField]
    private float baseFireRate = 0.5f;

    [SerializeField]
    private float baseCooldown = 2.0f;

    [SerializeField]
    private int baseMagazineSize = 10;

    [Header("Live Scalable Stats")]
    public float currentDamage;
    public float currentFireRate;
    public float currentReloadTime;
    public int currentMagazineSize;
    public int currentAmmo;
    public float nextReloadTime;

    void Awake()
    {
        currentDamage = baseDamage;
        currentFireRate = baseFireRate;
        currentReloadTime = baseCooldown;
        currentMagazineSize = baseMagazineSize;
        currentAmmo = currentMagazineSize;
        nextReloadTime = 0f;
    }

    public void ApplyDamageModifier(float multiplier)
    {
        currentDamage *= multiplier;
    }
}
