using UnityEngine;

public class Laser : Weapon
{
    [SerializeField] private LaserData laserWeapon;
    [SerializeField] private LineRenderer laserLine;
    private float currentHeat = 0f;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    private float sphereIntervalTimer = 2f;

    private void Start()
    {
        weapon = laserWeapon;
    }
    private void OnValidate()
    {
        weapon = laserWeapon;
    }
    protected override void Update()
    {
        base.Update();
        //Debug.Log($"current heat {currentHeat} Max Heat: {laserWeapon.GetMaxHeat(level, isOverDrive)}");
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
            laserLine.enabled = false;
            sphereIntervalTimer += Time.deltaTime;
            if(sphereIntervalTimer >= laserWeapon.GetCooldownDuration(level, isOverDrive))
            {
                laserWeapon.Fire(transform, level, isOverDrive);
                sphereIntervalTimer = 0f;
            }
        }
    }
    public override void StopFiring() {
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
        sphereIntervalTimer = 0f;
    }
}