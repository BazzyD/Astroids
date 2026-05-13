using UnityEngine;

public class OffscreenClamper : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Padding from the absolute screen edge")]
    [SerializeField] private float padding = 0.5f;

    [Header("Components")]
    [SerializeField] private SpriteRenderer arrowGraphic; // drag the IndicatorArrowGraphic here
    
    private Transform _parentAsteroid;

    private void Awake()
    {
        _parentAsteroid = transform.parent;
        if (arrowGraphic == null) arrowGraphic = GetComponentInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (ScreenBounds.Instance == null) return;

        // 1. Position of the parent in world space
        Vector3 parentWorldPos = _parentAsteroid.position;

        // 2. Check if the parent is outside the visible play area
        bool isXOutside = parentWorldPos.x < ScreenBounds.Instance.minX || parentWorldPos.x > ScreenBounds.Instance.maxX;
        bool isYOutside = parentWorldPos.y < ScreenBounds.Instance.minY || parentWorldPos.y > ScreenBounds.Instance.maxY;
        bool isOutsideVisibleArea = isXOutside || isYOutside;

        // 3. Enable/Disable the warning based on visibility
        if (!isOutsideVisibleArea)
        {
            arrowGraphic.enabled = false;
            return;
        }

        // --- If we get here, the asteroid is in the leeway zone ---
        arrowGraphic.enabled = true;

        // 4. Calculate the clamped position on the screen edge
        Vector3 clampedPos = parentWorldPos;

        // Clamp the world X/Y based on visible bounds, adjusted by padding
        clampedPos.x = Mathf.Clamp(clampedPos.x, ScreenBounds.Instance.minX + padding, ScreenBounds.Instance.maxX - padding);
        clampedPos.y = Mathf.Clamp(clampedPos.y, ScreenBounds.Instance.minY + padding, ScreenBounds.Instance.maxY - padding);

        // Update the Indicator's position in WORLD SPACE to be on the edge
        // (This makes this child object break away visually from its parent's position)
        transform.position = clampedPos;

        // 5. Handle Rotation: Make the tail of the arrow point *at the parent*
        // Direction from the clamped edge position BACK to the actual asteroid position
        Vector2 directionToOffscreenParent = (Vector2)(parentWorldPos - transform.position);

        // Math standard Atan2: standard sprites face Right, so we use Atan2(y,x).
        // Since we want the arrow (which faces Up) to align its Y-axis with that direction,
        // standard math for Up-facing sprites is: angle = Atan2(y,x) * Rad2Deg - 90f;
        // We want the arrow to point AT the asteroid, so we need: directionToOffscreenParent
        float angle = Mathf.Atan2(directionToOffscreenParent.y, directionToOffscreenParent.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}