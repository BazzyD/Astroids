using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewLaserData", menuName = "Weapons/Laser/WeaponData")]
public class LaserData : WeaponData
{
    public List<LaserLevelData> weaponLevels = new();
    public LaserLevelData overDriveLevelData;
    public override void Fire(Transform ship, int level, bool inOverDrive)
    {
        LaserLevelData levelData;
        if(inOverDrive) levelData = overDriveLevelData;
        else            levelData = weaponLevels[level];


        Vector3 offsetVector = Quaternion.Euler(0, 0, ship.eulerAngles.z) * Vector3.up * muzzeleOffset;
        Vector2 muzzelePosition = (Vector2)ship.position + (Vector2)offsetVector;
    }

}