using UnityEditor;
using UnityEngine;


public class WeaponHolder : MonoBehaviour{
    [Header("Fire Settings")]
    [SerializeField] private Transform muzzle;

    [Header("Weapon Data")]
    [SerializeField]private WeaponData weapon;
    public int level = 0;
    public bool isOverDrive = false;
    private float nextFireTime = 0f;
    private float overdriveTimer = 0f;
    private void Update(){
        if(isOverDrive)
        {
            HandleFire();
            overdriveTimer += Time.deltaTime;
            if(overdriveTimer >= weapon.overDriveDuration)
            {
                isOverDrive = false;
                overdriveTimer = 0f;
            }
        }
    }
    public void HandleFire()
    {
        float rate = weapon.GetFireRate(level, isOverDrive);

        if (Time.time >= nextFireTime) {
            nextFireTime = Time.time + rate;
            weapon.Fire(muzzle, level, isOverDrive);
        }
    }

    public void SetWeapon(WeaponData newWeapon)
    {
        // If the new weapon is the same as the current one, just level it up or enter overdrive
        if(weapon != null && weapon.weaponName == newWeapon.weaponName)
        {
            if(level <4)
                level++;
            else
                isOverDrive = true;
            return;
        }
        // Otherwise, switch to the new weapon level 0 and reset overdrive
        weapon = newWeapon;
        level = 0;
        isOverDrive = false;
        nextFireTime = 0f;
        overdriveTimer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        SpreadGunData spreadGunData = weapon as SpreadGunData;
        
        Vector3 offsetVector = Quaternion.Euler(0, 0, -spreadGunData.weaponLevels[level].startingAngle + transform.eulerAngles.z) * Vector3.up * spreadGunData.muzzeleOffset;
        Vector2 muzzelePosition = (Vector2)transform.position + (Vector2)offsetVector;

        Gizmos.DrawLine(transform.position, muzzelePosition);    
    }
}

