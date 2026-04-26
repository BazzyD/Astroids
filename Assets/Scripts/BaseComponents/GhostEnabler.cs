using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ScreenWrapper))]
public class GhostEnabler : MonoBehaviour
{
    private Collider2D _collider;
    private ScreenWrapper _wrapper;
    private float buffer = 0f;
    private void Awake(){
        _collider = GetComponent<Collider2D>();
        _wrapper = GetComponent<ScreenWrapper>();
        _collider.enabled = false;
    }
    private void Start(){
        _wrapper.enabled = true;
        buffer = _wrapper.getBuffer() - 0.02f;
        _wrapper.enabled = false;
    }
    private void Update(){
        if(_collider.enabled) return;

        Vector3 pos = transform.position;
        bool isInside = pos.x + buffer > ScreenBounds.Instance.minX &&
                          pos.x - buffer < ScreenBounds.Instance.maxX &&
                          pos.y + buffer > ScreenBounds.Instance.minY &&
                          pos.y - buffer < ScreenBounds.Instance.maxY;

        if(isInside){
                _collider.enabled = true;
                _wrapper.enabled = true;
                this.enabled = false;
        }
    }
}