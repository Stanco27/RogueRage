using UnityEngine;

public abstract class Coin : MonoBehaviour
{
    public enum CoinType
    {
        Currency,
        Experience,
    }

    [Header("Coin Properties")]
    public int value = 1;
    public CoinType type;

    private CoinGravity coinGravity;

    void Awake()
    {
        coinGravity = GetComponent<CoinGravity>();
    }

    public void Magentize(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            Collect(player.transform);
        }
        else
        {
            Debug.LogError("PlayerStats component not found on player object.");
        }
    }

    public void Collect(Transform target)
    {
        coinGravity.isMagnetized = true;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            20f * Time.deltaTime
        );
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            ApplyValue(target.GetComponent<PlayerStats>());
            Destroy(gameObject);
        }
    }

    protected abstract void ApplyValue(PlayerStats playerStats);
}
