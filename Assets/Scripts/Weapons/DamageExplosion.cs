using UnityEngine;

public class DamageExplosion : Explosion
{
    private float radius;
    private float damage;
    public override void OnDespawn()
    {
        radius = 0f;
        damage = 0f;
    }
    public void Initialize(float radius, float damage)
    {
        this.radius = radius;
        this.damage = damage;
    }
    public void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D hit in hits)
        {
            if(!hit.TryGetComponent(out Astroid astroid)) return;

            if(!astroid.TryGetComponent(out IDamageable health)) return;
            health.TakeDamage(damage);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
