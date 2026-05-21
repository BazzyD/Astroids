using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovment : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private float rotationSpeed = 50f;
    //[SerializeField] private float phoneThrustMultiplier = 0.8f;
    [SerializeField] private float maxSpeed = 10f;
    //[SerializeField] private float maxRotationSpeed = 100f;
    private float _thrustForce;
    private float _rotationTorque;
    private bool selfRotate = false;

    private Rigidbody2D rb;
    private Vector2 targetDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(selfRotate) return;
        if(GameManager.Instance.GetOnPhone()){
            if (targetDirection.magnitude > 0.1f)
            {
                // (Subtracting 90 degrees assumes your ship sprite naturally faces UP)
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
                
                // Smoothly rotate toward that target angle over time
                float smoothedAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed);
                transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
            }
        }
    }
    private void FixedUpdate()
    {
        if(selfRotate) return;
        if(GameManager.Instance.GetOnPhone()){
            if (targetDirection.magnitude > 0.1f)
            {
                rb.AddForce( thrustSpeed * transform.up);
            }
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
            if (!selfRotate)
            {
                rb.angularVelocity = 0f;
            }
            return;
        }
        HandleThrust();
        HandleRotation();
        
    }
    public void UpdateMovementInput(Vector2 dir){
        targetDirection = dir;
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
