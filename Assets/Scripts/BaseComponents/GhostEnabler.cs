using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ScreenWrapper))]
public class GhostEnabler : MonoBehaviour
{
    private Collider2D _collider;
    private ScreenWrapper _wrapper;
    //private float buffer = 0f;
    private void Awake(){
        _collider = GetComponent<Collider2D>();
        _wrapper = GetComponent<ScreenWrapper>();
        _collider.enabled = false;
        _wrapper.enabled = true;
    }
    private void OnBecameVisible()
    {
        _collider.enabled = true;
    }

    // Triggered when the object leaves all camera views
    private void OnBecameInvisible()
    {
        // Disable collider so it can't be hurt or hurt the player off-screen
        _collider.enabled = false;
    }
}