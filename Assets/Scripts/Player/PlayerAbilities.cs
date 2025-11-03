using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public PlayerStats playerStats;

    [Header("Dash Settings")]
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    public float dashSpeedMultiplier = 2f;
    private float dashDuration = 0.2f;

    private float dashTimer;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;

    public bool IsDashing => dashTimer > 0;

    void Update()
    {
        HandleDashMovement();
    }

    public void TryDash(Vector2 moveInput, Transform playerTransform)
    {
        if (!IsDashing && Time.time >= lastDashTime + dashCooldown)
        {
            lastDashTime = Time.time;

            Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
            dashDirection = playerTransform.TransformDirection(movement.normalized);

            if (dashDirection == Vector3.zero)
            {
                dashDirection = playerTransform.forward;
            }

            dashTimer = dashDuration;
        }
    }

    void HandleDashMovement()
    {
        if (dashTimer > 0)
        {
            float effectiveDashSpeed = playerStats.currentMoveSpeed * dashSpeedMultiplier;

            Vector3 dashVelocity = dashDirection * effectiveDashSpeed;

            if (controller != null)
            {
                controller.Move(dashVelocity * Time.deltaTime);
            }

            dashTimer -= Time.deltaTime;
        }
    }
}
