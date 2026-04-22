using System;
using UnityEngine;

public class Health : MonoBehaviour,  IDamageable {
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public bool IsInvincible { get; set; } = false;
    public float TotalFlightTime { get; private set; } = 0.4f;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Start(){
        currentHealth= maxHealth;
    }

    public void TakeDamage(float damageAmount){
        if(IsInvincible) return;
        
        currentHealth -= damageAmount;

        OnTakeDamage?.Invoke(damageAmount);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0){
            Die();
        }
    }
    public void Heal(float healAmount){
        currentHealth += healAmount;
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
