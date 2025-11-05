using UnityEngine;

public class CurrencyCoin : Coin
{
    private void OnValidate()
    {
        type = CoinType.Currency;
    }

    protected override void ApplyValue(PlayerStats playerStats)
    {
        Debug.Log($"Collected {value} Gold (Currency)!");
        playerStats.AddCurrency(value);
    }
}
