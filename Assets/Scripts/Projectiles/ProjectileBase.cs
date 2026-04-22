using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour, IPoolable
{
    [SerializeField] protected string poolTag = "Projectile";
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 10f;
    private float _lifeTimer;
    private bool _isDespawning = false;
    private Rigidbody2D _rb;

    protected virtual void Awake() => _rb = GetComponent<Rigidbody2D>();
    public virtual void OnSpawn()
    {
        _lifeTimer = lifetime;
        _isDespawning = false;
        _rb.linearVelocity = transform.up * speed;
    }
    protected virtual void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f && !_isDespawning)
        {
            _isDespawning = true;
            ObjectPool.Instance.Despawn(poolTag, gameObject);
        }
    }
    public virtual void OnDespawn()
    {
        _lifeTimer = 0f;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
        if(!_isDespawning){
            _isDespawning = true;
            ObjectPool.Instance.Despawn(poolTag, gameObject);
        }
    }
    protected virtual void HandleHit(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
