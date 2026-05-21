using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WeaponHolder : MonoBehaviour{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private CurrentWeaponData currentWeaponData;
    public Weapon CurrentWeapon => weapons[currentWeaponIndex];
    public int CurrentWeaponIndex => currentWeaponIndex;
    public Action<float,float> OnCooldownChanged;
    private void Start()
    {
        currentWeaponData.ResetWeapon();
        currentWeaponData.ChangeWeapon(currentWeaponIndex,CurrentWeapon.Level);
    }
    private void Update()
    {
        OnCooldownChanged?.Invoke(CurrentWeapon.GetCooldownDuration(), CurrentWeapon.GetCooldownTimer());
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
        if(CurrentWeapon.IsOverDrive) StartCoroutine(OverDriveSwapDelay(index));

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
    private IEnumerator OverDriveSwapDelay(int index)
    {
        while(CurrentWeapon.IsOverDrive)
        {
            yield return new WaitForSeconds(0.2f);
        }
        SwapWeapon(index);
    }
}

