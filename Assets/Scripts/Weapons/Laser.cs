using UnityEngine;

[RequireComponent(typeof(LaserAudioController))]
public class Laser : Weapon
{
    [SerializeField] private LaserData laserWeapon;
    [SerializeField] private LineRenderer laserLine;
    private LaserAudioController audioController;
    [SerializeField] private AudioClip laserSphereSound;
    
    private float currentHeat = 0f;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    private bool isSphereFiring = false;

    void Awake()
    {
        audioController = GetComponent<LaserAudioController>();
    }
    private void Start()
    {
        weapon = laserWeapon;
        overdriveTimer = weapon.overDriveDuration;
    }
    private void OnValidate()
    {
        weapon = laserWeapon;
    }
    protected override void Update()
    {
        base.Update();
        // cooling down
        if(isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer >= laserWeapon.GetCooldownDuration(level, isOverDrive))
            {
                isCoolingDown = false;
                cooldownTimer = 0f;
                currentHeat = 0f;
            }
        }
        else if(currentHeat > 0 && !isFiring)        {
            currentHeat -= Time.deltaTime;
            currentHeat = Mathf.Max(currentHeat, 0);
        }
        // fire
        else if (isFiring && !isOverDrive)
        {
            currentHeat += Time.deltaTime;
        }
    }
    public override void Fire()
    {
        if(isCoolingDown) return;


        isFiring = true;
        if(!isOverDrive){

            audioController.StartLaser();

            laserLine.enabled = true;
            
            laserWeapon.Fire(transform, level, isOverDrive, laserLine);
            
            float maxHeat = laserWeapon.GetMaxHeat(level, isOverDrive);
            if(maxHeat > 0 && currentHeat >= maxHeat)
            {
                isCoolingDown = true;
                StopFiring();
            }
        }
        else// in over drive
        {
            if(isSphereFiring)  return;

            laserLine.enabled = false;

            isSphereFiring=true;
            StartCoroutine(FireSpheres());
        }
    }
    private System.Collections.IEnumerator FireSpheres()
    {
        AudioManager.Instance.PlaySFX(laserSphereSound);
        laserWeapon.Fire(transform, level, isOverDrive);
        yield return new WaitForSeconds(laserWeapon.GetCooldownDuration(level, isOverDrive));

        AudioManager.Instance.PlaySFX(laserSphereSound);
        laserWeapon.Fire(transform, level, isOverDrive);
        yield return new WaitForSeconds(laserWeapon.GetCooldownDuration(level, isOverDrive));

        isSphereFiring=false;
    }
    public override void StopFiring() {
        if (isFiring)
        {
            audioController.StopLaser();
        }
        laserLine.enabled = false;
        isFiring = false;

    }
    public override void UpgradeWeapon()   
    {
        base.UpgradeWeapon();
    }
    public override void ResetWeapon()
    {
        base.ResetWeapon();
        currentHeat = 0f;
        cooldownTimer = 0f;
        isCoolingDown = false;
        isFiring = false;
        isSphereFiring = false;
        audioController?.StopLaser();
    }
    public override float GetCooldownTimer()
    {
        if(isCoolingDown)
            return cooldownTimer;

        float maxHeat = laserWeapon.GetMaxHeat(level, isOverDrive);
        return Mathf.Max(0f, maxHeat - currentHeat);
    }
    public override float GetCooldownDuration()
    {
        if(isCoolingDown)
            return laserWeapon.GetCooldownDuration(level, isOverDrive);

        return laserWeapon.GetMaxHeat(level, isOverDrive);
    }
}