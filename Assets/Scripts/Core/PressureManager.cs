using UnityEngine;
using System.Collections.Generic;
using System;


public class PressureManager : MonoBehaviour
{
    [SerializeField] private ScoreData astroidCount;
    public static PressureManager Instance;
    public  int CurrentLevel => currentLevel;
    public Action<int> OnChangeLevel;
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private List<int> pressurePerAstroidLevel = new() { 1, 5, 10, 25, 50 };
    private int currentAstroidToSpawn = 0;
    private int currentPressure = 0;

    private float nextSpawnTime = 0f;
    private bool isSpawnFinished = false;
    private bool isGameFinished = false;
    private void Awake(){
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start(){
        astroidCount.ResetScore();
        nextSpawnTime = Time.time;
        OnChangeLevel?.Invoke(currentLevel + 1);
    }
    private void Update(){
        if(isGameFinished) return;
        
        if (isSpawnFinished && currentPressure == 0 ) {
            isGameFinished = true;
            astroidCount.ResetScore();
            GameManager.Instance.WinGame();

        }
        
        if (isSpawnFinished) return;

        //if current level has more astroids to spawn
        if(currentLevel < levels.Count &&
           currentAstroidToSpawn < levels[currentLevel].astroidsToSpwan.Count)
        {
            SpawnAstroid();
        }
        else {// no more astroids to spawn in current level
            // is pressure under the minimum pressure for the level? if so, go to next level
            if( currentLevel < levels.Count &&
                currentAstroidToSpawn >= levels[currentLevel].astroidsToSpwan.Count&&
                currentPressure < levels[currentLevel].minPressure){
                currentLevel++;
                
                currentAstroidToSpawn = 0;

                if(currentLevel >= levels.Count) isSpawnFinished = true;
                else OnChangeLevel?.Invoke(currentLevel+1);
            }
        }
    }

    
    private void SpawnAstroid(){
        if (Time.time < nextSpawnTime) return;

        float maxPressure = levels[currentLevel].maxPressure;
        int astroidToSpwan = levels[currentLevel].astroidsToSpwan[currentAstroidToSpawn];
        float astroidToSpwanPressure = pressurePerAstroidLevel[astroidToSpwan-1];
        float nextPressure = currentPressure + astroidToSpwanPressure;

        // check if spawning the astroid will not exceed the max pressure for the level
        if(nextPressure <= maxPressure){

            Vector3 spawnPosition = ScreenBounds.GetRandomPosition(astroidToSpwan *6);
            Quaternion spawnRotation = ScreenBounds.GetRandomDirection(spawnPosition);
            ObjectPool.Instance.Spawn($"Astroid_lvl{astroidToSpwan}", spawnPosition, spawnRotation);
            
            currentAstroidToSpawn++;

            float spawnDelay = levels[currentLevel].spawnDelay;
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
    public void AddPressure(int astroidLevel) {
        astroidCount.AddScore(1);
        currentPressure += pressurePerAstroidLevel[astroidLevel-1];
    }
    public void RemovePressure(int astroidLevel) {
        astroidCount.AddScore(-1);
        currentPressure -= pressurePerAstroidLevel[astroidLevel-1];
    }
}