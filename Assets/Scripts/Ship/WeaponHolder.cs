using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class WeaponHolder : MonoBehaviour{
   [SerializeField] private List<Weapon> weapons = new List<Weapon>();
   [SerializeField] private int currentWeaponIndex = 0;
   private Weapon CurrentWeapon => weapons[currentWeaponIndex];

    public void HandleFire()
    {
        if(CurrentWeapon.IsOverDrive) return;
        CurrentWeapon.Fire();
    }
    public void SwapWeapon(int index)
    {
        if(index == currentWeaponIndex)
        {
            CurrentWeapon.UpgradeWeapon();
            return;
        }
        CurrentWeapon.ResetWeapon();
        currentWeaponIndex = index;
    }

}

