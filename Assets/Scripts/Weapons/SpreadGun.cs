using UnityEngine;

public class SpreadGun : Weapon
{
    [SerializeField] private SpreadGunData spreadGunweapon;
    [SerializeField] private AudioClip fireSound;
    private float nextFireTime = 0f;
    
    private void Start()
    {
        base.weapon = spreadGunweapon;
        overdriveTimer = weapon.overDriveDuration;
    }
    private void OnValidate()
    {
        base.weapon = spreadGunweapon;
    }

    public override void Fire()
    {
        if (Time.time < nextFireTime)  return;

        float rate = spreadGunweapon.GetFireRate(level, isOverDrive);
        nextFireTime = Time.time + rate;

        AudioManager.Instance.PlaySFX(fireSound);

        spreadGunweapon.Fire(this.transform, level, isOverDrive);
    }
    public override void StopFiring()
    {
        
    }
    public override void UpgradeWeapon()   
    {
        base.UpgradeWeapon();
    }
    public override void ResetWeapon()
    {
        base.ResetWeapon();
        nextFireTime = 0f;
    }
    public override float GetCooldownTimer()
    {
        return GetCooldownDuration() - Mathf.Max(0f, nextFireTime - Time.time);
    }
    public override float GetCooldownDuration()
    {
        return spreadGunweapon.GetFireRate(level, isOverDrive);
    }
}