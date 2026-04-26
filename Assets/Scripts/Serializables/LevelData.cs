using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "GameData/LevelData")]
public class LevelData : ScriptableObject
{
    public List<int> astroidsToSpwan = new();
    public int maxPressure =50;
    public int minPressure = 10;
    // How long to wait between spawns in this level
    public float spawnDelay = 2.0f;
}