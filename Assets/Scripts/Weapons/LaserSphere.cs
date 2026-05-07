using UnityEngine;

public class LaserSphere : MonoBehaviour
{
    [SerializeField] private float expandSpeed = 10f;
    [SerializeField] private float maxRadius = 15f;
    private float currentRadius = 0f;
    private float damage = 0f;

    void OnEnable()
    {
        currentRadius = 0f;
        transform.localScale = Vector3.zero;
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
            gameObject.SetActive(false); // Return to pool
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
