using System;
using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    protected WeaponData weapon;
    [SerializeField] protected int level = 0;
    [SerializeField] protected bool isOverDrive = false;
    
    public bool IsOverDrive => isOverDrive;
    protected float overdriveTimer = 0f;
    protected bool isFiring = false;
    public int Level => level;
    
    protected virtual void Update(){
        if(isOverDrive)
        {
            Fire();
            overdriveTimer -= Time.deltaTime;
            if(overdriveTimer <= 0f )
            {
                isOverDrive = false;
                overdriveTimer = weapon.overDriveDuration;
            }
        }
        
    }

    public abstract void Fire();
    public abstract void StopFiring();

    public virtual void UpgradeWeapon()   
    {
        if(level <4) level++;
        else isOverDrive = true;

        return;
    }
    public virtual void ResetWeapon()
    {
        StopFiring();
        level = 0;
        isOverDrive = false;
        overdriveTimer = weapon.overDriveDuration;
    }
    public virtual float GetCooldownTimer()
    {
        return overdriveTimer;
    }
    public virtual float GetCooldownDuration()
    {
        return weapon.overDriveDuration;
    }
}