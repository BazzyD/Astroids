using UnityEngine;

public class FitSpriteToScreen : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Reset scale to default
        transform.localScale = Vector3.one;

        // Get camera dimensions in world units
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Get sprite dimensions
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Calculate the exact scale required to cover the screen
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // Use the larger scale factor to ensure it completely envelopes the viewport (no gaps)
        float finalScale = Mathf.Max(scaleX, scaleY);
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
}
