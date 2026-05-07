using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject target;
    private float damage;
    private float currentSpeed=0;
    private float speed;
    private float maxSpeed;
    private float rotateSpeed;
    void Awake() => rb = GetComponent<Rigidbody2D>();
    public void Initialize(GameObject target,float dmg, float speed, float maxSpeed, float rotateSpeed)
    {
        this.target = target;
        this.damage = dmg;
        this.speed = speed;
        this.currentSpeed = speed;
        this.maxSpeed = maxSpeed;
        this.rotateSpeed = rotateSpeed;
        
        // Give it an initial "kick" forward
        rb.linearVelocity = transform.up * currentSpeed;
    }
    void FixedUpdate()
    {
        // 1. Move Forward
        rb.linearVelocity = transform.up * currentSpeed;
        
        // Accelerate up to max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += Time.fixedDeltaTime * speed;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
            
        // 2. Steering Logic
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if hit IDamageable, then deal damage and despawn
        if (other.TryGetComponent(out IDamageable astroid))
        {
            astroid.TakeDamage(damage);
            // Spawn explosion effect here?
            gameObject.SetActive(false); 
        }
    }
}