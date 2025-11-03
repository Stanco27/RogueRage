using UnityEngine;

public class Fists : MeleeWeapon
{
    protected override void Fire()
    {
        if (stats == null)
            return;
        CheckForHits();

        Debug.Log($"Fists hit check initiated. Damage: {stats.currentDamage}");
    }
}
