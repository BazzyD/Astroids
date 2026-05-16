using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownBar : MonoBehaviour
{
    [SerializeField] private Slider fill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fillImage;
    private WeaponHolder playerWeaponHolder;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerWeaponHolder = playerObj.GetComponent<WeaponHolder>();
    }
    private void Start()
    {
        float maxCooldown = playerWeaponHolder.CurrentWeapon.GetCooldownDuration();
        InitializeWeaponCooldownBar(maxCooldown); // Assuming GetMaxCooldown() returns the max cooldown time for the current weapon
    }
    private void OnEnable()
    {
        playerWeaponHolder.OnCooldownChanged += UpdateCooldownBar;
    }
    private void OnDisable()
    {
        playerWeaponHolder.OnCooldownChanged -= UpdateCooldownBar;
    }

    private void UpdateCooldownBar(float maxCooldown,float currentCooldown)
    {
        fill.maxValue = maxCooldown;
        fill.value = currentCooldown;
        fillImage.color = gradient.Evaluate(fill.normalizedValue);
    }
    private void InitializeWeaponCooldownBar(float maxCooldown)
    {
        fill.maxValue = maxCooldown;
        fill.value = maxCooldown;
        fillImage.color = gradient.Evaluate(1f);
    }

}
