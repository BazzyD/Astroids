using UnityEngine;
using System.Collections;

public class MissileLuncher : Weapon
{
    [SerializeField] private MissileData missileLuncherWeapon;
    private bool isVolleying = false;
    private float nextVolleyTime = 0f;
    private int missilenumber =0;

    private void Start()
    {
        base.weapon = missileLuncherWeapon;
    }
    private void OnValidate()
    {
        base.weapon = missileLuncherWeapon;
    }
    public override void Fire()
    {
        if (Time.time >= nextVolleyTime && !isVolleying)
        {
            StartCoroutine(LaunchVolley());
        }
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
            // 3. Tell Data to fire just ONE missile from the volley
            // We pass the specific target from our list
            GameObject target = (targets.Count > 0) ? targets[i % targets.Count] : null;
            missileLuncherWeapon.Fire(transform, level, isOverDrive, missilenumber,target);
            missilenumber++;
            // 4. THE INTERVAL: Wait before the next missile
            yield return new WaitForSeconds(stats.fireRate);
        }

        // 5. Start the main cooldown after the volley is finished
        nextVolleyTime = Time.time + stats.cooldownDuration;
        missilenumber =0;
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
        missilenumber =0;
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