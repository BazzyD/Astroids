using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance;
    public float minX, maxX, minY, maxY;
    [SerializeField] private float buffer = 4f;
    private Camera _camera;

    private void Awake(){
        if (Instance == null) {
            Instance = this;
            _camera = Camera.main;
            DontDestroyOnLoad(gameObject); 
        } else {
            Destroy(gameObject);
        }
    }

    private void Start(){
        // Get the screen corners in World Space
        // (0,0) is bottom-left, (Screen.width, Screen.height) is top-right
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;
    }
    public static Vector3 GetRandomPosition(){
        float leftEdge = Instance.minX - Instance.buffer;
        float rightEdge = Instance.maxX + Instance.buffer;
        float bottomEdge = Instance.minY - Instance.buffer;
        float topEdge = Instance.maxY + Instance.buffer;

        // Pick a random side (0=Top, 1=Bottom, 2=Left, 3=Right)
        int side = Random.Range(0, 4);

        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
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
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(0, 0, angle);
    }
    public static Vector3 GetRandomPointOnScreen(){
        float randomX = Random.Range(Instance.minX, Instance.maxX);
        float randomY = Random.Range(Instance.minY, Instance.maxY);
        
        return new Vector3(randomX, randomY, 0);
    }

}