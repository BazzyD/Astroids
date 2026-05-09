using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mine : MonoBehaviour, IPoolable
{
    private Rigidbody2D rb;
    [SerializeField] private string damageExplosionTag;
    [SerializeField] protected string poolTag = "Mine";
    private float explosionRadius;
    private float damage;
    private int minesToSpwan;
    private float size;
    private void Awake() => rb = GetComponent<Rigidbody2D>();
    public void Initialize(float damage, int minesToSpwan, float explosionRadius, float size)
    {
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.minesToSpwan = minesToSpwan;
        this.size = size;
        transform.localScale = new Vector3(size,size,0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
         // Check if hit IDamageable, then deal damage and despawn
        if (other.TryGetComponent(out IDamageable astroid))
        {
            
            astroid.TakeDamage(damage);

            if(ObjectPool.Instance == null) return;
            
            GameObject explosionObj = ObjectPool.Instance.Spawn(damageExplosionTag,transform.position,transform.rotation);
            
            if(!explosionObj.TryGetComponent(out DamageExplosion explosion)) return;
            explosion.Initialize(explosionRadius,damage);
            explosion.Explode();
            if(size>1)
                SpawnChildren();
            ObjectPool.Instance.Despawn(poolTag, gameObject);
        }
    }
    private void SpawnChildren(){
        for (int i = 0; i < minesToSpwan; i++)
        {
            float randomZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            if(ObjectPool.Instance != null){
                GameObject mineObj = ObjectPool.Instance.Spawn($"Mine", transform.position, randomRotation);
                if(!mineObj.TryGetComponent(out Mine mine)) return;
                int newSize = (int)size/2;
                mine.Initialize(damage/2, minesToSpwan/2, (int)explosionRadius/2, newSize > 0 ? newSize : 1f);
            }
        }
    }
    public  void OnSpawn()
    {
        rb.linearVelocity = transform.up * -0.5f;
    }
    public  void OnDespawn()
    {
        explosionRadius = 0f;
        damage = 0f;
        minesToSpwan = 0;
        rb.linearVelocity = Vector3.zero;
    }
}
