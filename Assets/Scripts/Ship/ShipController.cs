using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(BaseMovment))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShipController : MonoBehaviour{
    private InputHandler inputHandler;
    private BaseMovment baseMovment;
    private Health health;
    private WeaponHolder weaponHolder;
    private SpriteRenderer spriteRenderer;
    
    private PlayerStates currentState = PlayerStates.Idle;
    private bool _isHoldingFire;
    private float hurtTimer = 0f;
    [SerializeField]private float hurtDuration = 0.5f;

    [Header("Effects")]
    [SerializeField] private string explosionEffectTag = "Ship_Explosion";
    [SerializeField] private string hitEffectTag = "Ship_Hit";
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip destroySound;

    private void Awake(){
        inputHandler = GetComponent<InputHandler>();
        baseMovment = GetComponent<BaseMovment>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponHolder = GetComponent<WeaponHolder>();
    }
    private void OnEnable(){
        spriteRenderer.enabled = true;
        inputHandler.OnThrustChanged += SetThrust;
        inputHandler.OnRotationChanged += SetRotation;
        inputHandler.OnFirePerformed += OnFireInput;
        inputHandler.OnExplodePerformed += OnDeath;
        inputHandler.OnMovmentChange += UpdateMovementInput;

        health.OnTakeDamage += OnTakeDamage;
        health.OnDeath += OnDeath;
    }
    private void OnDisable(){
        inputHandler.OnThrustChanged -= SetThrust;
        inputHandler.OnRotationChanged -= SetRotation;
        inputHandler.OnFirePerformed -= OnFireInput;
        inputHandler.OnMovmentChange -= UpdateMovementInput;
        health.OnTakeDamage -= OnTakeDamage;
        health.OnDeath -= OnDeath;
    }

    private void UpdateMovementInput(Vector2 dir) => baseMovment.UpdateMovementInput(dir);

    private void SetThrust(float val) => baseMovment.SetForce(val);
    private void SetRotation(float val) => baseMovment.SetTorque(val);
    public void OnFireInput(bool pressed) 
    {
        _isHoldingFire = pressed;
    }
    private void OnTakeDamage(float damageAmount)
    {
        if(currentState == PlayerStates.Dead || currentState == PlayerStates.Hurt) return;
        
        currentState = PlayerStates.Hurt;
        _isHoldingFire = false;
        
        ObjectPool.Instance?.Spawn(hitEffectTag, transform.position, transform.rotation);
        AudioManager.Instance.PlaySFX(hitSound);

    }
    private void OnDeath()
    {
        if(currentState == PlayerStates.Dead) return;

        currentState = PlayerStates.Dead;
        _isHoldingFire = false;
        OnTakeDamage(0);
        spriteRenderer.enabled = false;
        AudioManager.Instance.PlaySFX(destroySound);
        StartCoroutine(ExplosionEffect());
    }
    private IEnumerator ExplosionEffect()
    {
        ObjectPool.Instance?.Spawn(explosionEffectTag, transform.position, transform.rotation);

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

        GameManager.Instance?.GameOver();
    }

    private void Update()
    {
        if(currentState == PlayerStates.Dead) return;
        
        if(currentState == PlayerStates.Hurt)
        {
            hurtTimer += Time.deltaTime;
            if(hurtTimer >= hurtDuration)
            {
                currentState = PlayerStates.Idle;
                hurtTimer = 0f;
                return;
            }
        }
        
        weaponHolder.HandleFire(_isHoldingFire);
    }

}