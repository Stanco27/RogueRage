using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [Header("Melee Properties")]
    public float attackRange = 2f;
    public float attackRadius = 1f;

    protected override void Fire()
    {
        CheckForHits();
    }

    protected virtual void CheckForHits()
    {
        Collider[] hitTargets = Physics.OverlapSphere(
            transform.position + transform.forward * (attackRange / 2f),
            attackRadius,
            LayerMask.GetMask("Enemy")
        );

        foreach (Collider hitCollider in hitTargets)
        {
            if (hitCollider.transform == transform.parent)
                continue;

            if (hitCollider.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                float currentWeaponDamage = stats != null ? stats.currentDamage : 0f;

                enemyHealth.TakeDamage(currentWeaponDamage, transform.position);
            }
        }
    }
}
