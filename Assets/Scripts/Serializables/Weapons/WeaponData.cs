using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public string weaponName;
    public float muzzeleOffset = 10f; // muzzle offset from the center of the ship
    public float overDriveDuration = 2f;
    public string projectilePrefabName;

    
    public abstract void Fire(Transform ship, int level, bool inOverDrive);
    public abstract float GetFireRate(int level, bool inOverDrive);

}