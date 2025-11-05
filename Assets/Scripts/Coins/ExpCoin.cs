using UnityEngine;

public class ExpCoin : Coin
{
    private void OnValidate()
    {
        type = CoinType.Experience;
    }

    protected override void ApplyValue(PlayerStats playerStats)
    {
        Debug.Log($"Collected {value} EXP!");
        playerStats.AddExperience(value);
    }
}
