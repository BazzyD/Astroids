using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSpreadGunData", menuName = "Weapons/SpreadGun/WeaponData")]
public class SpreadGunData : WeaponData
{
    public List<SpreadGunLevelData> weaponLevels = new();
    public SpreadGunLevelData overDriveLevelData;
    public void Fire(Transform ship, int level, bool inOverDrive)
    {
        SpreadGunLevelData levelData;
        if(inOverDrive) levelData = overDriveLevelData;
        else            levelData = weaponLevels[level];

        float currentAngle = levelData.startingAngle;
        for(int i=0; i<levelData.numberOfProjectiles; i++)
        {
            
            Vector3 offsetVector = Quaternion.Euler(0, 0, -currentAngle + ship.eulerAngles.z) * Vector3.up * muzzeleOffset;
            Vector2 muzzelePosition = (Vector2)ship.position + (Vector2)offsetVector;


            float bulletAngle = levelData.shootInAngle ? currentAngle : 0f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, -bulletAngle + ship.eulerAngles.z);
            ObjectPool.Instance.Spawn(projectilePrefabName, muzzelePosition, bulletRotation);
            
            currentAngle += levelData.spreadAngle;
        }
    }
    public float GetFireRate(int level, bool inOverDrive)
    {
        SpreadGunLevelData levelData;

        if(inOverDrive) levelData = overDriveLevelData;
        else levelData = weaponLevels[level];
        
        return levelData.fireRate;
    }
}