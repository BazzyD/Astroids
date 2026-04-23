using UnityEngine;

[RequireComponent(typeof(BaseMovment))]
public class Astroid : MonoBehaviour,IDamageable, IPoolable
{
    private BaseMovment _movment;

    [Header("Astroid Stats")]
    [SerializeField] public int astroidLevel =5;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageOnCollision = 25f;
    private float _currentHealth;
    [Header("Movement Stats")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxRotationSpeed = 200f;
    [SerializeField] private float minRotationSpeed = 100f;
    private int maxSpawnedAstroids = 5;
    private int minSpawnedAstroids = 2;
    public static System.Action<int> OnAsteroidKilled;
    private void Awake()
    {
        _movment = GetComponent<BaseMovment>();
    }

    public void OnSpawn()
    {
        _currentHealth = maxHealth;
        float speed = Random.Range(minSpeed, maxSpeed);
        float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        
        _movment.ApplyInitialImpulse(transform.up * speed);
        _movment.ApplyTorqueImpulse(rotationSpeed);
    }
    public void OnDespawn()
    {

        _movment.StopEverything();
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0)
        {
            OnAsteroidKilled?.Invoke(astroidLevel);
            if(astroidLevel > 1) SpawnChildren();
            ObjectPool.Instance.Despawn($"Astroid_lvl{astroidLevel}", gameObject);
        }
    }
    private void SpawnChildren(){
        int childrenAmount = Random.Range(minSpawnedAstroids, maxSpawnedAstroids+1);

        for (int i = 0; i < childrenAmount; i++)
        {
            float randomZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            ObjectPool.Instance.Spawn($"Astroid_lvl{astroidLevel - 1}", transform.position, randomRotation);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!(collision.gameObject.layer == LayerMask.NameToLayer("Player"))) return;
        
        if (collision.gameObject.TryGetComponent(out IDamageable player))
        {
            TakeDamage(maxHealth); // Destroy the astroid on collision with player
            player.TakeDamage(damageOnCollision);
        }
    }
}
