using UnityEngine;

public class ScoreManager : MonoBehaviour{
    [SerializeField] private ScoreData scoreData;
    //int[] scorePerLevel = new int[] { 100, 50, 30, 20, 10 };
    private void Start(){
        scoreData.ResetScore();
    }
    private void OnEnable(){
        Astroid.OnAsteroidKilled += HandleAsteroidKilled;
    }
    private void OnDisable(){
        Astroid.OnAsteroidKilled -= HandleAsteroidKilled;
    }
    private void HandleAsteroidKilled(int level){
        int points = 100 / level; 
        //int points = scorePerLevel[level-1];

        scoreData.AddScore(points);
    }
}