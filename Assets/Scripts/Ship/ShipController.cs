using UnityEngine;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(BaseMovment))]
[RequireComponent(typeof(Health))]
public class ShipController : MonoBehaviour{
    private InputHandler inputHandler;
    private BaseMovment baseMovment;
    private Health health;
    [SerializeField] private Transform muzzle;
    [Header("Fire Settings")]
    [SerializeField] private float fireRate = 0.2f;
    private float _nextFireTime;
    private bool _isHoldingFire;

    private void Awake(){
        inputHandler = GetComponent<InputHandler>();
        baseMovment = GetComponent<BaseMovment>();
        health = GetComponent<Health>();
    }
    private void OnEnable(){
        inputHandler.OnThrustChanged += SetThrust;
        inputHandler.OnRotationChanged += SetRotation;
        inputHandler.OnFirePerformed += OnFireInput;
        health.OnTakeDamage += OnTakeDamage;
        health.OnDeath += OnDeath;
    }
    private void OnDisable(){
        inputHandler.OnThrustChanged -= SetThrust;
        inputHandler.OnRotationChanged -= SetRotation;
        inputHandler.OnFirePerformed -= OnFireInput;
        health.OnTakeDamage -= OnTakeDamage;
        health.OnDeath -= OnDeath;
    }

    private void SetThrust(float val) => baseMovment.SetForce(val);
    private void SetRotation(float val) => baseMovment.SetTorque(val);
    public void OnFireInput(bool pressed) 
    {
        _isHoldingFire = pressed;
    }
    private void OnTakeDamage(float damageAmount)
    {
        _isHoldingFire = false;
    }
    private void OnDeath()
    {
        OnTakeDamage(0);
        GameManager.Instance.EndGame();
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