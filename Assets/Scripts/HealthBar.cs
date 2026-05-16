using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    private int maxHealth;

    public void Initialize(int maxValue)
    {
        maxHealth = maxValue;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        UpdateHealth(maxHealth);
    }

    public void UpdateHealth(int currentHealth)
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }
}