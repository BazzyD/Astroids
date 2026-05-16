using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    [Tooltip("0 for Ship/Mines (instant wrap), higher for Asteroids (leeway)")]
    [SerializeField] private float margin = 0f; 
    private Collider2D _collider;
    private Rigidbody2D rb;
    private float outsideTimer =0f;
    private float outsideDuration =10f;
    private bool _isInsideVisibleArea;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        outsideTimer = outsideDuration;
    }
    private void CheckVisibility()
    {
        // Manual check against the visible screen edges
        bool xInside = transform.position.x > ScreenBounds.Instance.minX && 
                       transform.position.x < ScreenBounds.Instance.maxX;
        bool yInside = transform.position.y > ScreenBounds.Instance.minY && 
                       transform.position.y < ScreenBounds.Instance.maxY;

        _isInsideVisibleArea = xInside && yInside;

        // Toggle collider based on visibility
        if (_collider.enabled != _isInsideVisibleArea)
        {
            _collider.enabled = _isInsideVisibleArea;
        }
    }

    private void Update()
    {
        CheckVisibility();
        if (!_isInsideVisibleArea)
        {
            // Only count down if we are off-screen
            outsideTimer -= Time.deltaTime;
            if (outsideTimer <= 0)
            {
                // RE-AIM: Pick a new direction toward the screen
                transform.rotation = ScreenBounds.GetRandomDirection(transform.position);
                float currentSpeed = rb.linearVelocity.magnitude;
                rb.linearVelocity = transform.up * currentSpeed;
                // RESET: Give it another full duration to try and enter the screen
                outsideTimer = outsideDuration;
            }
        }
        else
        {
            // If we are on-screen, keep the timer full and ready
            outsideTimer = outsideDuration;
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        // Get the current boundaries from our Singleton
        float left = ScreenBounds.Instance.minX - margin;
        float right = ScreenBounds.Instance.maxX + margin;
        float bottom = ScreenBounds.Instance.minY - margin;
        float top = ScreenBounds.Instance.maxY + margin;

        // Horizontal wrap
        if (pos.x < left) {
            pos.x = right;
        }
        else if (pos.x > right) {
            pos.x = left;
        }

        // Vertical wrap
        if (pos.y < bottom) {
            pos.y = top;
        }
        else if (pos.y > top) {
            pos.y = bottom;
        }

        transform.position = pos;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, margin);
    }
}
