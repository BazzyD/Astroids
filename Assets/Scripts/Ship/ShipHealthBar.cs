using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider fill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fillImage;
    private Health playerHealth;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerHealth = playerObj.GetComponent<Health>();
        
    }
    private void Start()
    {
        InitializeHealthBar(playerHealth.MaxHealth);
    }
    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth)
    {
        fill.value = currentHealth;
        fillImage.color = gradient.Evaluate(fill.normalizedValue);
    }
    private void InitializeHealthBar(float maxHealth)
    {
        fill.maxValue = maxHealth;
        fill.value = maxHealth;
        fillImage.color = gradient.Evaluate(1f); // Start with full health color
    }

}
