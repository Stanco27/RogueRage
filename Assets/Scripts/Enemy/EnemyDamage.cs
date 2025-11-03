using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float collisionDamage = 10f;
    public float damageCooldown = 0.5f;

    private float nextDamageTime;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time < nextDamageTime)
        {
            return;
        }

        if (
            hit.gameObject.TryGetComponent(out PlayerHealth victim)
            && hit.gameObject.CompareTag("Player")
        )
        {
            if (victim.gameObject != gameObject)
            {
                victim.TakeDamage(collisionDamage, transform.position);

                nextDamageTime = Time.time + damageCooldown;

                Debug.Log(
                    gameObject.name
                        + " hit "
                        + hit.gameObject.name
                        + " for "
                        + collisionDamage
                        + " damage."
                );
            }
        }
    }

}
