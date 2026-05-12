using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour{
    public static DropManager Instance;

    [SerializeField] private int nothingDropValue = 750;
    [SerializeField] private int healthDropValue = 900;
    //[SerializeField] private int weaponDropValue = 900;
    [SerializeField] List<GameObject> upgrades;
    [SerializeField] GameObject heal;

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
        else if(dropValue <= healthDropValue)
        {
            Instantiate(heal, position, Quaternion.identity);
        }
        else
        {
            int weaponIndex = Random.Range(0, 4);
            Instantiate(upgrades[weaponIndex], position, Quaternion.identity);
        }
    }

}