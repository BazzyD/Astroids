using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour{
    public static DropManager Instance;

    [SerializeField] private int nothingDropValue = 800;
    [SerializeField] private int healthDropValue = 900;

    private WeaponHolder player;
    
    private float dropTimer = 0f;
    private float dropCooldown = 3f;

    private float healDropTimer = 0f;

    private float weaponDropTimer = 0f;
    private float lastWeaponDropTime = 0f;

    private float[] weaponDropTimers;
    [SerializeField] List<string> upgrades;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<WeaponHolder>();
        weaponDropTimers = new float[upgrades.Count];
    }
    private void OnEnable(){
        Astroid.OnAsteroidKilled += HandleAsteroidKilled;
    }
    private void OnDisable(){
        Astroid.OnAsteroidKilled -= HandleAsteroidKilled;
    }
    private void HandleAsteroidKilled(int astroidLevel,Vector3 position)
    {
        if(dropTimer > 0f) return;

        // try to drop a weapon if the player hasent got one in the last 60 seconds,
        // if it drops return to prevent multiple drops from one asteroid
        if(DropWeapon(position))return;

        // (from 0-4) ^ 3 * 10 
        // e.g 0*0*0*10 = 0
        //     1*1*1*10 = 10
        //     2*2*2*10 = 80
        //     3*3*3*10 = 270
        //     4*4*4*10 = 640
        int startingValue = (int)Mathf.Pow(astroidLevel - 1, 3) * 10;
        int dropValue = Random.Range(startingValue, 1001);
        if(dropValue <= nothingDropValue) return;

        // got here something is dropping so reset the drop timer
        dropTimer = dropCooldown;

        if(dropValue <= healthDropValue )
        {
            if(healDropTimer <= 0f && ObjectPool.Instance != null){

                ObjectPool.Instance.Spawn("HealCollectable", position, Quaternion.identity);
                healDropTimer = dropCooldown;
            }
        }
        
        else if(weaponDropTimer <= 0f)
        {

            int weaponIndex = Random.Range(0, upgrades.Count);
            DroppingWeapon(position, weaponIndex);
        }
    }
    private void Update()
    {
        if(dropTimer > 0f) dropTimer -= Time.deltaTime;
        if(healDropTimer > 0f) healDropTimer -= Time.deltaTime;
        if(weaponDropTimer > 0f) weaponDropTimer -= Time.deltaTime;
    }

    // helper function to help the player get a weapon if he hasent got one in the last 60 seconds
    // returns true if a drop was successful
    private bool DropWeapon(Vector3 position)
    {
        // try to drop current weapon, if it drops return to prevent multiple drops from one asteroid
        if(DropCurrentWeapon(position)) return true;
        // prevent dropping if weapon has recently dropped (leave the rest to chance)
        if(Time.time - lastWeaponDropTime < 30f) return false; 


        //if we got here weapon wasnt dropped for the last 30 seconds
        int weaponIndex = Random.Range(0, upgrades.Count);
        
        DroppingWeapon(position, weaponIndex);

        return true;
    }

    // helper to help player drop their current weapon, returns true if a drop was successful
    private bool DropCurrentWeapon(Vector3 position)
    {
        if(player == null) return false;

        // dont drop if weapon is high enough level
        if(player.CurrentWeapon.Level > 3) return false; 

        // dont drop level is still low
        if(PressureManager.Instance != null && PressureManager.Instance.CurrentLevel < 4) return false; 

        int index = player.CurrentWeaponIndex;
        // dont drop if this weapon was dropped in the last 30 seconds
        if(Time.time - weaponDropTimers[index] < 30f) return false; 

        DroppingWeapon(position, index);

        return true;
    }
    private void DroppingWeapon(Vector3 position, int index)
    {
        ObjectPool.Instance.Spawn(upgrades[index], position, Quaternion.identity);

        weaponDropTimer = dropCooldown;
        lastWeaponDropTime = Time.time;
        weaponDropTimers[index] = Time.time;
    }
}