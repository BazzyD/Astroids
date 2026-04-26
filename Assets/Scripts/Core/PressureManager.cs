using UnityEngine;
using System.Collections.Generic;
using System;

public class PressureManager : MonoBehaviour
{
    public static PressureManager Instance;
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private List<int> pressurePerAstroidLevel = new() { 1, 5, 10, 25, 50 };
    private int currentAstroidToSpawn = 0;
    private int currentPressure = 0;

    private float nextSpawnTime = 0f;
    private bool isGameFinished = false;
    private void Awake()
    {
        // Standard Singleton Setup
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        nextSpawnTime = Time.time;
    }
    private void Update()
    {
        if (isGameFinished && currentPressure == 0) GameManager.Instance.EndGame();
        if (isGameFinished) return;
        Debug.Log($"Current Pressure: {currentPressure}");
        //if current level has more astroids to spawn
        if(currentAstroidToSpawn < levels[currentLevel].astroidsToSpwan.Count)
        {
            SpawnAstroid();
        }
        else // no more astroids to spawn in current level
        {
            // is pressure under the minimum pressure for the level? if so, go to next level
            if(currentPressure < levels[currentLevel].minPressure)
            {
                Debug.Log("Next level!");
                currentLevel++;
                currentAstroidToSpawn = 0;
                if(currentLevel >= levels.Count)
                {
                    isGameFinished = true;
                    Debug.Log("All levels completed!");
                }
            }
        }
    }

    private void SpawnAstroid()
    {
        if (Time.time < nextSpawnTime) return;

        float maxPressure = levels[currentLevel].maxPressure;
        int astroidToSpwan = levels[currentLevel].astroidsToSpwan[currentAstroidToSpawn];
        float astroidToSpwanPressure = pressurePerAstroidLevel[astroidToSpwan-1];
        float nextPressure = currentPressure + astroidToSpwanPressure;

        Debug.Log($"Trying to spawn {nextPressure} pressure if spawned");
        // check if spawning the astroid will not exceed the max pressure for the level
        if(nextPressure <= maxPressure){

            Vector3 spawnPosition = ScreenBounds.GetRandomPosition();
            Quaternion spawnRotation = ScreenBounds.GetRandomDirection(spawnPosition);

            ObjectPool.Instance.Spawn($"Astroid_lvl{astroidToSpwan}", spawnPosition, spawnRotation);
            
            
            currentAstroidToSpawn++;

            float spawnDelay = levels[currentLevel].spawnDelay;
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    public void AddPressure(int astroidLevel)
    {
        currentPressure += pressurePerAstroidLevel[astroidLevel-1];
    }
    public void RemovePressure(int astroidLevel)
    {
        currentPressure -= pressurePerAstroidLevel[astroidLevel-1];;
    }
}
