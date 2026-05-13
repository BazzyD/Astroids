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
        if(currentState == PlayerStates.Dead || currentState == PlayerStates.Hurt) return;
        
        currentState = PlayerStates.Hurt;
        _isHoldingFire = false;
        if(ObjectPool.Instance != null)
                ObjectPool.Instance.Spawn(hitEffectTag, this.transform.position, this.transform.rotation);

    }
    private void OnDeath()
    {
        if(currentState == PlayerStates.Dead) return;

        currentState = PlayerStates.Dead;
        _isHoldingFire = false;
        OnTakeDamage(0);
        spriteRenderer.enabled = false;
        StartCoroutine(ExplosionEffect());
    }
    private IEnumerator ExplosionEffect()
    {
        if(ObjectPool.Instance != null)
                ObjectPool.Instance.Spawn(explosionEffectTag, this.transform.position, this.transform.rotation);

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        if(GameManager.Instance != null)
            GameManager.Instance.GameOver();
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