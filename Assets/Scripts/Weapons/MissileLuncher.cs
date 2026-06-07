using UnityEngine;
using System.Collections;

public class MissileLuncher : Weapon
{
    [SerializeField] private MissileData missileLuncherWeapon;
    [SerializeField] private AudioClip fireSound;
    private bool isVolleying = false;
    private float nextVolleyTime = 0f;

    private void Start()
    {
        base.weapon = missileLuncherWeapon;
        overdriveTimer = weapon.overDriveDuration;
    }
    private void OnValidate()
    {
        base.weapon = missileLuncherWeapon;
    }
    public override void Fire()
    {
        if (Time.time < nextVolleyTime || isVolleying) return;
        
        StartCoroutine(LaunchVolley());
    }
    private IEnumerator LaunchVolley()
    {
        isVolleying = true;
        
        // Get the current level stats
        MissileLevelData stats = isOverDrive ? missileLuncherWeapon.overDriveLevelData : missileLuncherWeapon.weaponLevels[level];
        
        // 2. Ask the Data for targets ONCE at the start of the volley
        var targets = missileLuncherWeapon.GetTargets(transform, level, isOverDrive);

        for (int i = 0; i < stats.missilesAmount; i++)
        {
            // Fire ONE missile from the volley withthe specific target
            GameObject target = (targets.Count > 0) ? targets[i % targets.Count] : null;

            // Play sound for each missile launch
            AudioManager.Instance.PlaySFX(fireSound);

            missileLuncherWeapon.Fire(transform, level, isOverDrive,target);
            // Wait before the next missile
            yield return new WaitForSeconds(stats.fireRate);
        }

        // Start the main cooldown after the volley is finished
        nextVolleyTime = Time.time + stats.cooldownDuration;
        isVolleying = false;
    }
    public override void StopFiring() {}
    public override void UpgradeWeapon()   
    {
        base.UpgradeWeapon();
    }
    public override void ResetWeapon()
    {
        base.ResetWeapon();
        isVolleying = false;
        nextVolleyTime = 0f;
    }
    public override float GetCooldownTimer()
    {
        if (isVolleying)
        {
            return 0f;
        }
        return missileLuncherWeapon.GetCooldownDuration(level, isOverDrive) - Mathf.Max(0f, nextVolleyTime - Time.time);
    }
    public override float GetCooldownDuration()
    {
        return missileLuncherWeapon.GetCooldownDuration(level, isOverDrive); 
    }
}