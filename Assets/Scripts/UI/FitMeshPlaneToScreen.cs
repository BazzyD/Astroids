using UnityEngine;

public class FitMeshPlaneToScreen : MonoBehaviour
{
    private void Start()
    {
        // CRITICAL: A standard Unity Plane is 10x10 units at scale 1
        float planeBaseSize = 10f; 

        Camera cam = Camera.main;
        if (cam == null) return;
        
        float worldWidth;
        float worldHeight;
        
        // 1. Calculate screen size in world units based on camera type
        if (cam.orthographic)
        {
            worldHeight = cam.orthographicSize * 2f;
            worldWidth = worldHeight * cam.aspect;
        }
        else
        {
            // Perspective Camera math: finds the screen size at the exact depth of the plane
            float distance = Mathf.Abs(transform.position.z - cam.transform.position.z);
            worldHeight = 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            worldWidth = worldHeight * cam.aspect;
        }

        // 2. Determine scale requirements relative to the 10-unit mesh size
        float requiredScaleX = worldWidth / planeBaseSize;
        float requiredScaleZ = worldHeight / planeBaseSize; 

        // 3. Prevent aspect-ratio stretching (Enveloping method)
        // This picks the larger scale factor to guarantee the plane overflows the edges
        // on ultra-wide screens rather than leaving black gaps.
        float finalScale = Mathf.Max(requiredScaleX, requiredScaleZ);

        // 4. Apply the scale
        // Because the plane is rotated 90 degrees on X, local Z controls visual height!
        transform.localScale = new Vector3(finalScale, 1f, finalScale);
    }
}