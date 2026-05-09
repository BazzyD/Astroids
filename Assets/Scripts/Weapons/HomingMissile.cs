using UnityEngine;

public class HomingMissile : ProjectileBase
{
    private GameObject target;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private string damageExplosionTag;
    private float rotateSpeed;
    private bool isOverdrive = false;

    public void Initialize(GameObject target,float damage, float speed, float rotateSpeed, bool isOverdrive)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
        this.isOverdrive = isOverdrive;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
        if (target != null && target.activeInHierarchy)
        {
            Vector2 direction = (Vector2)target.transform.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
        }
        else
        {
            // If target is lost, stop turning and just fly straight
            rb.angularVelocity = 0;
        }
    }

    protected override void HandleHit(Collider2D other)
    {
        Debug.Log(isOverdrive);
        // Check if hit IDamageable, then deal damage and despawn
        if (other.TryGetComponent(out IDamageable astroid))
        {
            astroid.TakeDamage(damage);

            if(ObjectPool.Instance == null) return;
            
            if(isOverdrive){
                Debug.Log("GotHere");
                GameObject explosionObj = ObjectPool.Instance.Spawn(damageExplosionTag,transform.position,transform.rotation);
                
                if(!explosionObj.TryGetComponent(out DamageExplosion explosion)) return;
                explosion.Initialize(explosionRadius,damage);
                explosion.Explode();
            }
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        target = null;
        rotateSpeed = 0f;
        isOverdrive = false;
    }
}