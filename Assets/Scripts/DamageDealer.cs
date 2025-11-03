using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 25f;
    public bool destroyOnHit = true;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth targetHealth = collision.gameObject.GetComponent<EnemyHealth>();

        Vector3 damageSourcePosition = collision.contacts[0].point;

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount, damageSourcePosition);
        }

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

}
