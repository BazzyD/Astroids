using System;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour, ICollectable
{
    [SerializeField] private int weaponIndex;
    public void Collect(GameObject collector)
    {
        if(!collector.TryGetComponent(out WeaponHolder playerWeapon)) return;

        playerWeapon.SwapWeapon(weaponIndex);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Collect(other.gameObject);
    }
}