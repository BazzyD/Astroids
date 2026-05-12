using UnityEngine;

[RequireComponent(typeof(BaseMovment))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class Astroid : MonoBehaviour, IPoolable
{
    public static System.Action<int, Vector3> OnAsteroidKilled;

    private BaseMovment _movment;
    private SpriteRenderer spriteRenderer;
    private Health health;
    private bool isDespawning = false;
    private int maxSpawnedAstroids = 5;
    private int minSpawnedAstroids = 2;
    

    [Header("Astroid Stats")]
    [SerializeField] public int astroidLevel = 5;
    [SerializeField] private float damageOnCollision = 25f;
    [SerializeField] private string explosionEffectTag = "Astroid_Explosion";
    
    [Header("Movement Stats")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxRotationSpeed = 200f;
    [SerializeField] private float minRotationSpeed = 100f;
    
    private void Awake()
    {
        _movment = GetComponent<BaseMovment>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        health.Heal(health.MaxHealth);
        health.OnTakeDamage += TakeDamage;
        health.OnDeath += Death;
    }
    private void OnDisable()
    {
        health.OnTakeDamage -= TakeDamage;
        health.OnDeath -= Death;
    }
    public void OnSpawn()
    {
        isDespawning = false;
        spriteRenderer.enabled=true;
        if(PressureManager.Instance != null)
            PressureManager.Instance.AddPressure(astroidLevel);

        health.Heal(health.MaxHealth);
        health.SetInvincible();
        float speed = Random.Range(minSpeed, maxSpeed);
        float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        
        _movment.ApplyInitialImpulse(transform.up * speed);
        _movment.ApplyTorqueImpulse(rotationSpeed);

    }
    public void OnDespawn()
    {
        if(PressureManager.Instance != null)
            PressureManager.Instance.RemovePressure(astroidLevel);
        _movment.StopEverything();
    }

    public void TakeDamage(float damageAmount)
    {
        if(isDespawning) return;
    }
    private void Death()
    {
        isDespawning = true;
        spriteRenderer.enabled = false;
        OnAsteroidKilled?.Invoke(astroidLevel,transform.position);
        if(astroidLevel > 1) SpawnChildren();
        if(ObjectPool.Instance != null){
            ObjectPool.Instance.Spawn(explosionEffectTag, this.transform.position, this.transform.rotation);
            ObjectPool.Instance.Despawn($"Astroid_lvl{astroidLevel}", gameObject);
        }
    }

    private void SpawnChildren(){
        int childrenAmount = Random.Range(minSpawnedAstroids, maxSpawnedAstroids+1);

        for (int i = 0; i < childrenAmount; i++)
        {
            float randomZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            if(ObjectPool.Instance != null)
                ObjectPool.Instance.Spawn($"Astroid_lvl{astroidLevel - 1}", transform.position, randomRotation);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!(collision.gameObject.layer == LayerMask.NameToLayer("Player"))) return;
        
        if (collision.gameObject.TryGetComponent(out IDamageable player))
        {
            Death();
            player.TakeDamage(damageOnCollision);
        }
    }
}
