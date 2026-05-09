using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour,  IDamageable {
    private Collider2D damageCollider;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibleDuration =1f;
    private float currentHealth;
    public bool IsInvincible { get; set; } = false;
    private float invincibleTimer =0f;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake(){
        currentHealth= maxHealth;
        damageCollider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        damageCollider.enabled =true;
        currentHealth = MaxHealth;
    }
    private void Update()
    {
        if(invincibleTimer <= 0) return;

        invincibleTimer -= Time.deltaTime;
        if(invincibleTimer <= 0)
        {
            damageCollider.enabled = true;
            IsInvincible =false;
        }
    }
    public void SetInvincible()
    {
        invincibleTimer = invincibleDuration;
        damageCollider.enabled = false;
        IsInvincible =true;
    }

    public void TakeDamage(float damageAmount){
        if(IsInvincible) return;
        
        invincibleTimer = invincibleDuration;
        damageCollider.enabled = false;
        IsInvincible =true;

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
    }
}
