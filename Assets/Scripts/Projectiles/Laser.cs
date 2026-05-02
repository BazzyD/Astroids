using UnityEngine;

public class Laser : Weapon
{
    [SerializeField] private LaserData laserWeapon;
    [SerializeField] private LineRenderer laserLine;
    private float currentHeat = 0f;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;

    private void Start()
    {
        base.weapon = laserWeapon;
    }
    private void OnValidate()
    {
        base.weapon = laserWeapon;
    }
    protected override void Update()
    {
        base.Update();
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
    }
    public override void Fire()
    {
        if(isCoolingDown) return;
        isFiring = true;
        laserLine.enabled = true;
        
        currentHeat += Time.deltaTime;
        laserWeapon.FireLaser(this.transform, level, isOverDrive, laserLine);
        if(currentHeat >= laserWeapon.GetMaxHeat(level, isOverDrive))
        {
            isCoolingDown = true;
            StopFiring();
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
    }
}