using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // =======================================================================
    // REFERENCES & ESSENTIAL
    // =======================================================================

    [Header("References")]
    public Transform playerCamera;
    public PlayerStats playerStats;
    public PlayerAbilities playerAbilities;

    // =======================================================================
    // SETTINGS
    // =======================================================================

    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float jumpHeight = 1f;

    [SerializeField]
    private float gravity = -9.81f;

    public float mouseSensitivity = 150f;
    public float lookSmoothTime = 0.05f;

    [Header("Dash Settings")]
    public float dashDistance = 5f;
    public float dashCooldown = 1f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private RangedWeaponManager rangedManager;
    private MeleeWeaponManager meleeManager;

    [Header("Camera Shake Settings")]
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.1f;

    [Header("Collection Settings")]
    public LayerMask collectibleLayer;

    // =======================================================================
    // PRIVATE STATE VARIABLES
    // =======================================================================

    private CharacterController playerController;
    private Vector2 moveInput;
    private Vector3 playerVelocity;

    private Vector2 lookInput;
    private Vector2 lookSmoothVelocity;
    private Vector2 currentLookVector;
    private float xRotation = 0f;

    private float knockbackTimer;
    private Vector3 knockbackDirection;
    private float shakeTimer;
    public Fists fistsWeapon;

    private bool isFireHeld = false;

    // =======================================================================
    // UNITY LIFECYCLE METHODS
    // =======================================================================

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        GameObject managerObject = GameObject.Find("WeaponManager");
        if (managerObject != null)
        {
            rangedManager = managerObject.GetComponent<RangedWeaponManager>();
            meleeManager = managerObject.GetComponent<MeleeWeaponManager>();
        }
        else
        {
            Debug.LogError("GameManager object not found! Managers cannot be initialized.");
        }

        Cursor.visible = false;

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.identity;

            xRotation = 0f;
        }
    }

    void Update()
    {
        ApplyGravity();

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (playerAbilities == null || !playerAbilities.IsDashing)
        {
            float speed = playerStats != null ? playerStats.currentMoveSpeed : 5f;

            playerVelocity.x = move.x * speed;
            playerVelocity.z = move.z * speed;
        }
        ApplyKnockback();
        playerController.Move(playerVelocity * Time.deltaTime);
        if (isFireHeld && rangedManager != null)
        {
            rangedManager.CheckAndFireManualWeapon();
        }
        HandleCoinCollection();
    }

    void LateUpdate()
    {
        HandleLookRotation();
    }

    // =======================================================================
    // INPUT ACTION CALLBACKS
    // =======================================================================

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Melee(InputAction.CallbackContext context)
    {
        if (context.performed && fistsWeapon != null)
        {
            fistsWeapon.TryFire();
            Debug.Log("Melee attack attempted.");
        }
        Debug.Log("Melee input received.");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && playerController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFireHeld = true;
        }
        if (context.canceled)
        {
            isFireHeld = false;
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && playerAbilities != null)
        {
            playerAbilities.TryDash(moveInput, transform);
        }
    }

    // =======================================================================
    // CORE MOVEMENT & ROTATION LOGIC
    // =======================================================================

    void HandleLookRotation()
    {
        if (playerCamera == null)
            return;

        currentLookVector = Vector2.SmoothDamp(
            currentLookVector,
            lookInput,
            ref lookSmoothVelocity,
            lookSmoothTime
        );

        float mouseX = currentLookVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = currentLookVector.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void ApplyGravity()
    {
        if (playerController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
    }

    // =======================================================================
    // DAMAGE FEEDBACK LOGIC
    // =======================================================================

    public void ApplyKnockbackAndShake(Vector3 damageSourcePosition)
    {
        knockbackDirection = (transform.position - damageSourcePosition).normalized;
        knockbackDirection.y = 0.5f;
        knockbackTimer = knockbackDuration;
    }

    private void ApplyKnockback()
    {
        if (knockbackTimer > 0)
        {
            Vector3 knockbackVelocity = knockbackDirection * knockbackForce;
            playerVelocity.x += knockbackVelocity.x;
            playerVelocity.z += knockbackVelocity.z;

            knockbackTimer -= Time.deltaTime;
        }
    }

    private void HandleCoinCollection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            playerStats.collectionRadius,
            collectibleLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            Coin coinComponent = hitCollider.GetComponent<Coin>();

            if (coinComponent != null)
            {
                coinComponent.Magentize(gameObject);
            }
        }
    }
}
