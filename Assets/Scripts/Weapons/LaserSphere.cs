using UnityEngine;

public class LaserSphere : MonoBehaviour, IPoolable
{
    [SerializeField] private float expandSpeed = 10f;
    [SerializeField] private float maxRadius = 15f;
    private float currentRadius = 0f;
    private float damage = 0f;

    public void OnSpawn()
    {
        currentRadius = 0f;
        transform.localScale = Vector3.zero;
    }
    public void OnDespawn()
    {
        
    }
    public void Initialize(float dmg)
    {
        damage = dmg;
    }
    void Update()
    {
        currentRadius += expandSpeed * Time.deltaTime;

        transform.localScale = new Vector3(currentRadius, currentRadius, 1f);


        if (currentRadius >= maxRadius)
        {
            if(ObjectPool.Instance == null) return;
            ObjectPool.Instance.Despawn("Laser_Shpere",gameObject);
        }
    }

    //evrything that thouch the collider and has an IHurable recive damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable victim))
        {
            victim.TakeDamage(damage);
        }
    }
}
