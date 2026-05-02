using UnityEngine;

public class SpreadGun : Weapon
{
    [SerializeField] private SpreadGunData spreadGunweapon;
    private float nextFireTime = 0f;
    
    private void Start()
    {
        base.weapon = spreadGunweapon;
    }
    private void OnValidate()
    {
        base.weapon = spreadGunweapon;
    }
    public override void Fire()
    {
        float rate = spreadGunweapon.GetFireRate(level, isOverDrive);

        if (Time.time >= nextFireTime) {
            nextFireTime = Time.time + rate;
            spreadGunweapon.Fire(this.transform, level, isOverDrive);
        }
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
}