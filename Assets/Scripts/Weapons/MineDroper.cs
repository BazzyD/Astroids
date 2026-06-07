using UnityEngine;

public class MineDroper : Weapon
{
    [SerializeField] private MineData mineDroperWeapon;
    [SerializeField] private AudioClip fireSound;
    private float nextFireTime = 0f;

    private void Start()
    {
        base.weapon = mineDroperWeapon;
        overdriveTimer = weapon.overDriveDuration;
    }
    private void OnValidate()
    {
        base.weapon = mineDroperWeapon;
    }


    public override void Fire()
    {
        if (Time.time < nextFireTime) return;
        
        AudioManager.Instance.PlaySFX(fireSound);
        
        mineDroperWeapon.Fire(transform, level, isOverDrive);
        nextFireTime = Time.time + mineDroperWeapon.GetFireRate(level, isOverDrive);
        
    }
    public override void StopFiring() {}
    public override void UpgradeWeapon()   
    {
        base.UpgradeWeapon();
        nextFireTime = 0;
    }
    public override float GetCooldownTimer()
    {
        return  GetCooldownDuration() - Mathf.Max(0f, nextFireTime - Time.time);
    }
    public override float GetCooldownDuration()
    {
        return mineDroperWeapon.GetFireRate(level, isOverDrive);
    }
}
