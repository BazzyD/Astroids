using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;


[CreateAssetMenu(fileName = "NewLaserData", menuName = "Weapons/Laser/WeaponData")]
public class LaserData : WeaponData
{
    public List<LaserLevelData> weaponLevels = new();
    public LaserLevelData overDriveLevelData;
    public void FireLaser(Transform ship, int level, bool inOverDrive, LineRenderer line)
    {
        LaserLevelData levelData = inOverDrive ? overDriveLevelData : weaponLevels[level];


        Vector3 offsetVector = Quaternion.Euler(0, 0, ship.eulerAngles.z) * Vector3.up * muzzeleOffset;
        Vector2 muzzelePosition = (Vector2)ship.position + (Vector2)offsetVector;

        Vector2 direction = ship.up;

        float maxDistance = 50f; // Far enough to go off screen
        RaycastHit2D hit = Physics2D.BoxCast(muzzelePosition, new Vector2(levelData.width, 0.1f), ship.eulerAngles.z, direction, maxDistance);
        
        float beamLength = maxDistance;

        if (hit.collider != null)
        {
            beamLength = hit.distance;

            // 4. Apply Damage
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                // IMPORTANT: Since Fire() is called every frame, we multiply damage by Time.deltaTime
                damageable.TakeDamage(levelData.damage * Time.deltaTime);
            }
        }
        line.SetPosition(0, muzzelePosition);
        line.SetPosition(1, muzzelePosition + (direction * beamLength));
        

        line.startWidth = levelData.width; 
        line.endWidth = levelData.width;
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