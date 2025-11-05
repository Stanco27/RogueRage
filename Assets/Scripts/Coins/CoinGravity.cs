using UnityEngine;

public class CoinGravity : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float fixedFallSpeed = 10f;

    [Header("Detection")]
    public float checkRadius = 0.1f;
    public LayerMask groundMask;

    private bool hasLanded = false;
    public bool isMagnetized = false;

    void Awake()
    {
        if (groundMask.value == 0)
        {
            groundMask = LayerMask.GetMask("Ground");
        }
    }

    void OnEnable()
    {
        hasLanded = false;
    }

    void Update()
    {
        if (hasLanded || isMagnetized)
            return;
        transform.Translate(Vector3.down * fixedFallSpeed * Time.deltaTime, Space.World);

        CheckForGround();
    }

    void CheckForGround()
    {
        if (Physics.CheckSphere(transform.position, checkRadius, groundMask))
        {
            hasLanded = true;
        }
    }
}
