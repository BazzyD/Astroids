using UnityEngine;
using System.Collections;

public class MineDroper : Weapon
{
    [SerializeField] private MineData mineDroperWeapon;
    private bool isVolleying = false;
    private float nextVolleyTime = 0f;
    private int minenumber =0;

    private void Start()
    {
        base.weapon = mineDroperWeapon;
    }
    private void OnValidate()
    {
        base.weapon = mineDroperWeapon;
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
        MineLevelData stats = isOverDrive ? mineDroperWeapon.overDriveLevelData : mineDroperWeapon.weaponLevels[level];

        for (int i = 0; i < stats.minesAmount; i++)
        {
            // 3. Tell Data to fire just ONE mine from the volley
            // We pass the specific target from our list
            mineDroperWeapon.Fire(transform, level, isOverDrive, minenumber);
            minenumber++;
            // 4. THE INTERVAL: Wait before the next mine
            yield return new WaitForSeconds(stats.fireRate);
        }

        // 5. Start the main cooldown after the volley is finished
        nextVolleyTime = Time.time + stats.cooldownDuration;
        minenumber =0;
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
        minenumber =0;
        isVolleying = false;
        nextVolleyTime = 0f;
    }
}
