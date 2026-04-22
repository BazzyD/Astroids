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
        rb.AddForce(_thrustForce * thrustSpeed * transform.up);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    private void HandleRotation()
    {
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
    public void SetForce(float force)
    {
        _thrustForce = force;
    }
    public void SetTorque(float torque)
    {
        _rotationTorque = torque;
    }
}
