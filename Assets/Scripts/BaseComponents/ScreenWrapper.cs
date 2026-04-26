using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField] private float buffer = 0.05f; 

    // private void Start()
    // {
    //     get the sprite size in world units to use as a buffer for wrapping
    //     buffer = GetComponent<SpriteRenderer>().bounds.extents.x;
    // }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Horizontal wrap
        if (pos.x < ScreenBounds.Instance.minX - buffer) pos.x = ScreenBounds.Instance.maxX + buffer;
        else if (pos.x > ScreenBounds.Instance.maxX + buffer) pos.x = ScreenBounds.Instance.minX - buffer;

        // Vertical wrap
        if (pos.y < ScreenBounds.Instance.minY - buffer) pos.y = ScreenBounds.Instance.maxY + buffer;
        else if (pos.y > ScreenBounds.Instance.maxY + buffer) pos.y = ScreenBounds.Instance.minY - buffer;

        transform.position = pos;
    }
    public float getBuffer()
    {
        return buffer;
    }
}
