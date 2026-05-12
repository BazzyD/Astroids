using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMineData", menuName = "Weapons/MineDroper/WeaponData")]
public class MineData : WeaponData
{
    public List<MineLevelData> weaponLevels = new();
    public MineLevelData overDriveLevelData;
    public override void Fire(Transform ship, int level, bool inOverDrive, params object[] args)
    {
        MineLevelData levelData = inOverDrive ? overDriveLevelData : weaponLevels[level];
        

        Vector3 rearPosition = ship.position - (ship.up * muzzeleOffset);

        if(ObjectPool.Instance == null) return;


        GameObject mineObj = ObjectPool.Instance.Spawn(projectilePrefabName,rearPosition,Quaternion.Euler(0, 0, ship.eulerAngles.z));
        
        if(!mineObj.TryGetComponent(out Mine mine)) return;
        
        mine.Initialize(levelData.damage, levelData.minesToSpwan,levelData.explosionRadius,levelData.mineSize);
        
    }
    public float GetFireRate(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.fireRate : weaponLevels[level].fireRate;
    }
    public float GetCooldownDuration(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.cooldownDuration : weaponLevels[level].cooldownDuration;
    }
    public float GetMineAmount(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.minesAmount : weaponLevels[level].minesAmount;
    }

}