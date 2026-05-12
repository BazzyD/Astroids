using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour, IPoolable
{
    protected Rigidbody2D rb;

    [SerializeField] protected string poolTag = "Projectile";
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 5f;
    [SerializeField] protected float damage = 10f;

    protected float _lifeTimer;
    protected bool _isDespawning = false;
    

    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();
    public virtual void OnSpawn()
    {
        _lifeTimer = lifetime;
        _isDespawning = false;
        rb.linearVelocity = transform.up * speed;
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
        rb.linearVelocity = Vector2.zero;
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
