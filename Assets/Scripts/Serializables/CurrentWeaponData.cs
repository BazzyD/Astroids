using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrentWeaponData", menuName = "GameData/CurrentWeaponData")]
public class CurrentWeaponData : ScriptableObject
{
    public int currentWeaponIndex =0;
    public int currentWeaponLevel =0;

    public System.Action<int, int> OnWeaponChanged;

    public void ChangeWeapon(int index, int level)
    {
        currentWeaponIndex = index;
        currentWeaponLevel = level;
        OnWeaponChanged?.Invoke(index,level);
    }

    public void ResetWeapon()
    {
        currentWeaponIndex = 0;
        currentWeaponLevel = 0;
        OnWeaponChanged?.Invoke(currentWeaponIndex,currentWeaponLevel);
    }
}