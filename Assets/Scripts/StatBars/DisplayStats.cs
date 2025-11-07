using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI HealthText;

    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI killsText;

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
                playerStats.OnCurrencyChanged.AddListener(UpdateCurrencyDisplay);
                playerStats.OnExperienceChanged.AddListener(UpdateExperienceDisplay);
                playerStats.OnKillsChanged.AddListener(UpdateKillsDisplay);

                UpdateHealthDisplay(playerHealth.GetNormalizedHealth());
                UpdateCurrencyDisplay(playerStats.currentCurrency);
                UpdateExperienceDisplay(playerStats.currentExperience);
                UpdateKillsDisplay(playerStats.killCount);
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
            Debug.LogError("Player object with tag 'Player' not found. HUD disabled.");
        }
    }

    public void UpdateHealthDisplay(float normalizedHealth)
    {
        if (Slider != null)
        {
            Slider.value = normalizedHealth;
        }

        if (HealthText != null && playerStats != null)
        {
            string current = playerStats.currentHealth.ToString("F2");
            string max = playerStats.currentMaxHealth.ToString("F2");

            HealthText.text = $"{current} / {max}";
        }
    }

    public void UpdateCurrencyDisplay(float newCurrency)
    {
        if (currencyText != null)
        {
            currencyText.text = $"{Mathf.RoundToInt(newCurrency)}"; 
        }
    }

    public void UpdateExperienceDisplay(float newExperience)
    {
        if (experienceText != null)
        {
            experienceText.text = $"EXP: {newExperience.ToString("F2")}"; 
        }
    }

    public void UpdateKillsDisplay(int newKillCount)
    {
        if (killsText != null)
        {
            Debug.Log("Kills HUD updated to: " + newKillCount);
            killsText.text = $"{newKillCount}";
        }
    }
}