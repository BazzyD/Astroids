using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance;
    [Header("Screen Stats")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private Camera _camera;

    private void Awake(){
        if (Instance == null) {
            Instance = this;
            _camera = Camera.main;
            UpdateBounds();
        } else {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Calculates the world space coordinates of the screen edges.
    /// </summary>
    public void UpdateBounds()
    {
        if (_camera == null) _camera = Camera.main;
        // Get the screen corners in World Space
        // (0,0) is bottom-left, (Screen.width, Screen.height) is top-right
        // For ScreenToWorldPoint to work correctly, the Z value 
        // should be the distance from the camera to the object plane (usually 10).
        float screenHeight = _camera.orthographicSize * 2f;
        float screenWidth = screenHeight * _camera.aspect;

        float camX = _camera.transform.position.x;
        float camY = _camera.transform.position.y;

        minX = camX - (screenWidth / 2f);
        maxX = camX + (screenWidth / 2f);
        minY = camY - (screenHeight / 2f);
        maxY = camY + (screenHeight / 2f);
    }
    public static Vector3 GetRandomPosition(float customBuffer){
        if (Instance == null) return Vector3.zero;

        Vector3 spawnPos = Vector3.zero;
        float leftEdge = Instance.minX - customBuffer;
        float rightEdge = Instance.maxX + customBuffer;
        float bottomEdge = Instance.minY - customBuffer;
        float topEdge = Instance.maxY + customBuffer;

        // Pick a random side (0=Top, 1=Bottom, 2=Left, 3=Right)
        int side = Random.Range(0, 4);
        switch (side){
            case 0: // Top
                spawnPos = new Vector3(Random.Range(leftEdge, rightEdge), topEdge, 0);
                break;
            case 1: // Bottom
                spawnPos = new Vector3(Random.Range(leftEdge, rightEdge), bottomEdge, 0);
                break;
            case 2: // Left
                spawnPos = new Vector3(leftEdge, Random.Range(bottomEdge, topEdge), 0);
                break;
            case 3: // Right
                spawnPos = new Vector3(rightEdge, Random.Range(bottomEdge, topEdge), 0);
                break;
        }
        return spawnPos;
    }
    public static Quaternion GetRandomDirection(Vector3 pos){
        Vector3 targetPos = GetRandomPointOnScreen();
        Vector3 direction = (targetPos - pos).normalized;
        
        // Simplified rotation for 2D (assuming the sprite faces 'Up')
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(0, 0, angle);
    }
    public static Vector3 GetRandomPointOnScreen(){
        if (Instance == null) return Vector3.zero;

        float randomX = Random.Range(Instance.minX, Instance.maxX);
        float randomY = Random.Range(Instance.minY, Instance.maxY);
        
        return new Vector3(randomX, randomY, 0);
    }
}