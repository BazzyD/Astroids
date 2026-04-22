using UnityEngine;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(BaseMovment))]
public class ShipController : MonoBehaviour{
    private InputHandler inputHandler;
    private BaseMovment baseMovment;
    [SerializeField] private Transform muzzle;
    [Header("Fire Settings")]
    [SerializeField] private float fireRate = 0.2f;
    private float _nextFireTime;
    private bool _isHoldingFire;

    private void Awake(){
        inputHandler = GetComponent<InputHandler>();
        baseMovment = GetComponent<BaseMovment>();
    }
    private void OnEnable(){
        inputHandler.OnThrustChanged += SetThrust;
        inputHandler.OnRotationChanged += SetRotation;
        inputHandler.OnFirePerformed += OnFireInput;
    }
    private void OnDisable(){
        inputHandler.OnThrustChanged -= SetThrust;
        inputHandler.OnRotationChanged -= SetRotation;
        inputHandler.OnFirePerformed -= OnFireInput;
    }

    private void SetThrust(float val) => baseMovment.SetForce(val);
    private void SetRotation(float val) => baseMovment.SetTorque(val);
    public void OnFireInput(bool pressed) 
    {
        _isHoldingFire = pressed;
    }

    private void Update()
    {
        if (_isHoldingFire && Time.time >= _nextFireTime)
        {
            HandleFire();
            _nextFireTime = Time.time + fireRate;
        }
    }
    private void HandleFire()
    {
        ObjectPool.Instance.Spawn("Projectile", muzzle.position, muzzle.rotation);
    }
}