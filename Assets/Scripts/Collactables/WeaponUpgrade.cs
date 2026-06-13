using UnityEngine;

public class WeaponUpgrade : CollectableBase
{
    [SerializeField] private int weaponIndex;
    
    public override void Collect(GameObject collector)
    {
        if(!collector.TryGetComponent(out WeaponHolder playerWeapon)) return;

        playerWeapon.SwapWeapon(weaponIndex);
        
        base.Collect(collector);
    }

}