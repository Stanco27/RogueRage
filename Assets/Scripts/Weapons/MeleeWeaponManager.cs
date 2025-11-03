using UnityEngine;

public class MeleeWeaponManager : MonoBehaviour
{
    [Header("Melee Slots")]
    public Weapon manualMeleeWeapon;
    public Weapon[] autoMeleeWeapons;

    void Update()
    {
        foreach (Weapon weapon in autoMeleeWeapons)
        {
            if (weapon != null)
            {
                weapon.TryFire();
            }
        }
    }

    public void TryManualMelee()
    {
        if (manualMeleeWeapon != null)
        {
            manualMeleeWeapon.TryFire();
        }
    }
}
