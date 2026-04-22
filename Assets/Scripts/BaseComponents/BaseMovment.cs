using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovment : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float maxSpeed = 10f;
    //[SerializeField] private float maxRotationSpeed = 100f;
    private float _thrustForce;
    private float _rotationTorque;
    private bool selfRotate = false;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        HandleThrust();
        HandleRotation();
        
    }
    private void HandleThrust()
    {
        if(selfRotate) return;
        rb.AddForce(_thrustForce * thrustSpeed * transform.up);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    private void HandleRotation()
    {
        if(selfRotate) return;
        rb.angularVelocity = _rotationTorque * -rotationSpeed;
        // rb.AddTorque(_rotationInput * -rotationSpeed);
        // if (rb.angularVelocity > maxRotationSpeed)
        // {
        //     rb.angularVelocity = maxRotationSpeed;
        // }
        // if (rb.angularVelocity < -maxRotationSpeed)
        // {
        //     rb.angularVelocity = -maxRotationSpeed;
        // }
    }
    public void ApplyInitialImpulse(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ApplyTorqueImpulse(float torque)
    {
        // Randomize clockwise or counter-clockwise
        selfRotate = true;
        float direction = Random.value > 0.5f ? 1f : -1f;
        rb.AddTorque(torque * direction, ForceMode2D.Impulse);
    }
    public void SetForce(float force)
    {
        _thrustForce = force;
    }
    public void SetTorque(float torque)
    {
        _rotationTorque = torque;
    }
    public void StopEverything()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
