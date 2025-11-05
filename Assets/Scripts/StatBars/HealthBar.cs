using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI HealthText;

    private PlayerHealth playerHealth;
    private PlayerStats playerStats;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerStats = player.GetComponent<PlayerStats>();

            if (playerHealth != null && playerStats != null)
            {
                Slider.maxValue = 1f;
                playerHealth.OnHealthChanged.AddListener(UpdateHealthDisplay);
                UpdateHealthDisplay(playerHealth.GetNormalizedHealth());
            }
            else
            {
                Debug.LogError(
                    "PlayerHealth or PlayerStats component not found on Player object. HUD disabled."
                );
            }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found.");
        }
    }

    public void UpdateHealthDisplay(float normalizedHealth)
    {
        Slider.value = normalizedHealth;

        if (playerStats != null)
        {
            string current = playerStats.currentHealth.ToString("F2");
            string max = playerStats.currentMaxHealth.ToString("F2");

            HealthText.text = $"{current} / {max}";
        }
        else
        {
            HealthText.text = $"{(normalizedHealth * 100f).ToString("F0")}%";
        }
    }
}
