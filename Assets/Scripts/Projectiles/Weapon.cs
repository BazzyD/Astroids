using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    protected WeaponData weapon;
    [SerializeField] protected int level = 0;
    [SerializeField] protected bool isOverDrive = false;
    public bool IsOverDrive => isOverDrive;
    protected float overdriveTimer = 0f;
    
    protected virtual void Update(){
        if(isOverDrive)
        {
            Fire();
            overdriveTimer += Time.deltaTime;
            if(overdriveTimer >= weapon.overDriveDuration)
            {
                isOverDrive = false;
                overdriveTimer = 0f;
            }
        }
    }

    public abstract void Fire();

    public virtual void UpgradeWeapon()   
    {
        if(level <4) level++;
        else isOverDrive = true;

        return;
    }
    public virtual void ResetWeapon()
    {
        level = 0;
        isOverDrive = false;
        overdriveTimer = 0f;
    }
}