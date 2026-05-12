using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLaserData", menuName = "Weapons/Laser/WeaponData")]
public class LaserData : WeaponData
{
    public List<LaserLevelData> weaponLevels = new();
    public LaserLevelData overDriveLevelData;
    public LayerMask layerMask;
    public override void Fire(Transform ship, int level, bool inOverDrive, params object[] args)
    {
        
        if(inOverDrive)
            OverDriveLaser(ship);
        else
            NormalLaser(ship, level, args[0] as LineRenderer);

    }
    private void NormalLaser(Transform ship, int level, LineRenderer line)
    {
        LaserLevelData levelData = weaponLevels[level];

        Vector3 offsetVector = Quaternion.Euler(0, 0, ship.eulerAngles.z) * Vector3.up * (muzzeleOffset+level/2);
        Vector2 muzzelePosition = (Vector2)ship.position + (Vector2)offsetVector;

        Vector2 direction = ship.up;

        float maxDistance = 50f; // Far enough to go off screen
        RaycastHit2D hit = Physics2D.BoxCast(muzzelePosition, new Vector2(levelData.width, 0.1f), ship.eulerAngles.z, direction, maxDistance, layerMask);
        
        float beamLength = maxDistance;

        if (hit.collider != null)
        {
            beamLength = hit.distance;

            // 4. Apply Damage
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(levelData.damage);
            }
        }
        line.SetPosition(0, muzzelePosition);
        line.SetPosition(1, muzzelePosition + (direction * beamLength));
        
        // Set the width based on the level data
        line.widthMultiplier = levelData.width;
    }
    private void OverDriveLaser(Transform ship)
    {
        LaserLevelData levelData = overDriveLevelData;
        if(ObjectPool.Instance != null){
            GameObject sphere = ObjectPool.Instance.Spawn(projectilePrefabName, ship.position, Quaternion.identity);
            if(sphere.TryGetComponent(out LaserSphere ls))
            {
                ls.Initialize(levelData.damage);
            }
        }
    }
    public float GetCooldownDuration(int level, bool inOverDrive)
    {
        if(inOverDrive) return overDriveLevelData.cooldownDuration;
        else            return weaponLevels[level].cooldownDuration;
    }
    public float GetMaxHeat(int level, bool inOverDrive)
    {
        if(inOverDrive) return overDriveLevelData.maxHeat;
        else            return weaponLevels[level].maxHeat;
    }

}