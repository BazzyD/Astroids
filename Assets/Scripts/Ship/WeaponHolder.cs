using System;
using System.Collections.Generic;
using UnityEngine;


public class WeaponHolder : MonoBehaviour{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private CurrentWeaponData currentWeaponData;
    private Weapon CurrentWeapon => weapons[currentWeaponIndex];
    private void Start()
    {
        currentWeaponData.ResetWeapon();
        currentWeaponData.ChangeWeapon(currentWeaponIndex,CurrentWeapon.Level);
    }
    public void HandleFire(bool isFiring)
    {
        if(CurrentWeapon.IsOverDrive) return;
        if(isFiring){
            CurrentWeapon.Fire();
        }
        else
        {
            CurrentWeapon.StopFiring();
        }
    }
    public void SwapWeapon(int index)
    {
        if(index == currentWeaponIndex)
        {
            CurrentWeapon.UpgradeWeapon();
            currentWeaponData.ChangeWeapon(currentWeaponIndex,CurrentWeapon.Level);
            return;
        }
        CurrentWeapon.ResetWeapon();
        currentWeaponIndex = index;
        currentWeaponData.ChangeWeapon(currentWeaponIndex,CurrentWeapon.Level);
    }
}

