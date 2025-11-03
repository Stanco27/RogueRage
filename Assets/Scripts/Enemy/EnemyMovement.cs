using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform playerTransform;

    private CharacterController controller;

    [Header("Movement Settings")]
    [Tooltip("Maximum speed of the enemy.")]
    public float movementSpeed = 3.5f;

    [Tooltip("The distance at which the enemy stops to attack or orbit.")]
    public float stoppingDistance = 1.5f;

    [Tooltip("How quickly the enemy turns to face the player.")]
    public float rotationSpeed = 5f;

    [Tooltip("The actual distance to stop movement and keep pushing for collision.")]
    public float approachThreshold = 0.1f;

    private float gravity = -10f;
    private Vector3 currentVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError(
                "CharacterController component is missing on "
                    + gameObject.name
                    + ". Please attach one.",
                this
            );
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError(
                "Player not found! Ensure your player has the 'Player' tag set in the Inspector.",
                this
            );
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stoppingDistance + approachThreshold)
        {
            ChaseTarget(movementSpeed);
        }
        else if (distanceToPlayer > approachThreshold)
        {
            ChaseTarget(movementSpeed * 0.5f);
        }
        else
        {
            currentVelocity.x = 0;
            currentVelocity.z = 0;
        }

        RotateTowardsPlayer();
        ApplyMovementAndGravity();
    }


    private void ChaseTarget(float speed)
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;

        currentVelocity.x = flatDirection.x * speed;
        currentVelocity.z = flatDirection.z * speed;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void ApplyMovementAndGravity()
    {
        if (controller.isGrounded)
        {
            currentVelocity.y = -0.5f;
        }
        else
        {
            currentVelocity.y += gravity * Time.deltaTime;
        }

        controller.Move(currentVelocity * Time.deltaTime);
    }
}
