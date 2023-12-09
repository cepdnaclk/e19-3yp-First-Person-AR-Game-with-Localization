using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider healthSlider;
    public Image redSplatter;
    public Image whiteGradient;
    public TMP_Text gameOverText;

    [SerializeField] private float maxHealth = 1.0f;
    [SerializeField] private float minHealth = 0.0f;

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        gameOverText.enabled = false;
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float currentHealth = healthSlider.value;
        float normalizedHealth = Mathf.Clamp01((currentHealth - minHealth) / (maxHealth - minHealth));

        // Calculate the opacity based on health
        float redSplatterOpacity = 1.0f - normalizedHealth;
        float whiteGradientOpacity = normalizedHealth;

        // Set the alpha values for red splatter and white gradient
        Color redSplatterColor = redSplatter.color;
        redSplatterColor.a = redSplatterOpacity;
        redSplatter.color = redSplatterColor;

        Color whiteGradientColor = whiteGradient.color;
        whiteGradientColor.a = whiteGradientOpacity;
        whiteGradient.color = whiteGradientColor;
        if (currentHealth <= minHealth)
        {
            gameOverText.enabled = true;
        }
        
    }
}
