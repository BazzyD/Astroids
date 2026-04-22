using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField] private float buffer = 0.05f; 
    private float _minX, _maxX, _minY, _maxY;
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        // get the sprite size in world units to use as a buffer for wrapping
        //buffer = GetComponent<SpriteRenderer>().bounds.extents.x;
        
        // Get the screen corners in World Space
        // (0,0) is bottom-left, (Screen.width, Screen.height) is top-right
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        _minX = bottomLeft.x;
        _maxX = topRight.x;
        _minY = bottomLeft.y;
        _maxY = topRight.y;
    }
    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Horizontal wrap
        if (pos.x < _minX - buffer) pos.x = _maxX + buffer;
        else if (pos.x > _maxX + buffer) pos.x = _minX - buffer;

        // Vertical wrap
        if (pos.y < _minY - buffer) pos.y = _maxY + buffer;
        else if (pos.y > _maxY + buffer) pos.y = _minY - buffer;

        transform.position = pos;
    }
}
