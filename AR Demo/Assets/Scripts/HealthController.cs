using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider healthSlider, healthSlider2;
    public Image redSplatter, redSplatter2;
    public Image whiteGradient, whiteGradient2;
    public TMP_Text gameOverText, gameOverText2;

    [SerializeField] private float maxHealth = 1.0f;
    [SerializeField] private float minHealth = 0.0f;

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider2.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        healthSlider2.value = maxHealth;
        gameOverText.enabled = false;
        gameOverText2.enabled = false;
    }

    private void Update()
    {
        UpdateHealth();
        healthSlider.value = healthSlider2.value;
    }

    private void UpdateHealth()
    {
        float currentHealth = healthSlider.value;
        healthSlider2.value = healthSlider.value;
        float normalizedHealth = Mathf.Clamp01((currentHealth - minHealth) / (maxHealth - minHealth));

        // Calculate the opacity based on health
        float redSplatterOpacity = 1.0f - normalizedHealth;
        float whiteGradientOpacity = normalizedHealth;

        // Set the alpha values for red splatter and white gradient
        Color redSplatterColor = redSplatter.color;
        redSplatterColor.a = redSplatterOpacity;
        redSplatter.color = redSplatterColor;
        redSplatter2.color = redSplatterColor;

        Color whiteGradientColor = whiteGradient.color;
        whiteGradientColor.a = whiteGradientOpacity;
        whiteGradient.color = whiteGradientColor;
        whiteGradient2.color = whiteGradientColor;
        if (currentHealth <= minHealth)
        {
            gameOverText.enabled = true;
            gameOverText2.enabled = true;
        }

    }
}