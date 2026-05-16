using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour{
    public static DropManager Instance;

    [SerializeField] private int nothingDropValue = 850;
    [SerializeField] private int healthDropValue = 950;
    //[SerializeField] private int weaponDropValue = 900;
    private float healDropTimer = 0f;
    private float healDropCooldown = 3f;
    private float weaponDropTimer = 0f;
    private float weaponDropCooldown = 3f;
    [SerializeField] List<string> upgrades;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void OnEnable(){
        Astroid.OnAsteroidKilled += HandleAsteroidKilled;
    }
    private void OnDisable(){
        Astroid.OnAsteroidKilled -= HandleAsteroidKilled;
    }
    private void HandleAsteroidKilled(int astroidLevel,Vector3 position)
    {
        // (from 0-4) ^ 3 * 10 
        // e.g 0*0*0*10 = 0
        //     1*1*1*10 = 10
        //     2*2*2*10 = 80
        //     3*3*3*10 = 270
        //     4*4*4*10 = 640
        int startingValue = (int)Mathf.Pow(astroidLevel - 1, 3) * 10;
        int dropValue = Random.Range(startingValue, 1001);
        if(dropValue <= nothingDropValue) return;
        else if(dropValue <= healthDropValue && healDropTimer <= 0f)
        {
            ObjectPool.Instance.Spawn("HealCollectable", position, Quaternion.identity);
            healDropTimer = healDropCooldown;
        }
        else if(weaponDropTimer <= 0f)
        {
            int weaponIndex = Random.Range(0, 4);
            ObjectPool.Instance.Spawn(upgrades[weaponIndex], position, Quaternion.identity);
            weaponDropTimer = weaponDropCooldown;
        }
    }
    private void Update()
    {
        if(healDropTimer > 0f) healDropTimer -= Time.deltaTime;
        if(weaponDropTimer > 0f) weaponDropTimer -= Time.deltaTime;
    }

}