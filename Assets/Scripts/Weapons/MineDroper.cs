using UnityEngine;

public class MineDroper : Weapon
{
    [SerializeField] private MineData mineDroperWeapon;
    private float nextFireTime = 0f;

    private void Start()
    {
        base.weapon = mineDroperWeapon;
    }
    private void OnValidate()
    {
        base.weapon = mineDroperWeapon;
    }


    public override void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            mineDroperWeapon.Fire(transform, level, isOverDrive);
            nextFireTime = Time.time + mineDroperWeapon.GetFireRate(level, isOverDrive);
        }
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
