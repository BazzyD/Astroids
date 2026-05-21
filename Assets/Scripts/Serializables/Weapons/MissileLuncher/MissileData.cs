using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewMissileData", menuName = "Weapons/MissileLuncher/WeaponData")]
public class MissileData : WeaponData
{
    public List<MissileLevelData> weaponLevels = new();
    public MissileLevelData overDriveLevelData;
    public LayerMask targetLayer;
    public float maxRadius = 50f;
    public override void Fire(Transform ship, int level, bool inOverDrive, params object[] args)
    {
        MissileLevelData levelData = inOverDrive ? overDriveLevelData : weaponLevels[level];
        
        GameObject target = args[1] as GameObject;

        Vector3 muzzelePosition = ship.position + (ship.up * muzzeleOffset);

        if(ObjectPool.Instance == null) return;

        GameObject missileObj = ObjectPool.Instance.Spawn(projectilePrefabName,muzzelePosition,Quaternion.Euler(0, 0, ship.eulerAngles.z));
        
        if(!missileObj.TryGetComponent(out HomingMissile missile)) return;
        
        missile.Initialize(target,levelData.damage,levelData.flightSpeed, levelData.rotateSpeed, inOverDrive);
        
    }
    public List<GameObject> GetTargets(Transform ship,int level, bool inOverDrive)
    {
        MissileLevelData levelData = inOverDrive ? overDriveLevelData : weaponLevels[level];
        Vector3 muzzelePosition = ship.position + (ship.up * muzzeleOffset);
        Collider2D[] hits = Physics2D.OverlapCircleAll(muzzelePosition, maxRadius, targetLayer);
        var sortedTargets = hits
            .OrderBy(h => (h.transform.position - ship.position).sqrMagnitude)
            .Take(levelData.missilesAmount)
            .Select(h => h.gameObject)
            .ToList();

        return sortedTargets;
    }
    public float GetFireRate(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.fireRate : weaponLevels[level].fireRate;
    }
    public float GetCooldownDuration(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.cooldownDuration : weaponLevels[level].cooldownDuration;
    }
    public float GetMissileAmount(int level, bool inOverDrive)
    {
        return inOverDrive ? overDriveLevelData.missilesAmount : weaponLevels[level].missilesAmount;
    }

}