using UnityEngine;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour{
    public static SaveManager Instance;
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private TimerData timerData;
        private void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void OnEnable(){
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }
    private void OnDisable(){
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStates gameState){
        if(gameState == GameStates.GameOver){
            SaveData();
        }
    }
    private void SaveData(){
        int score = scoreData.currentScore;
        float time = timerData.currentTime;
        SaveEntry data = new(){
            score = score,
            time = time
        };

        LeaderboardData leaderboard = LoadData();
        leaderboard.entries.Add(data);

        leaderboard.entries = leaderboard.entries
        .OrderByDescending(e => e.score)
        .Take(10)
        .ToList();

        string json = JsonUtility.ToJson(leaderboard, true); 
        File.WriteAllText(GetPath(), json);
    }
    private static string GetPath(){
        return Path.Combine(Application.persistentDataPath, "leaderboard.json");
    }
    public static LeaderboardData LoadData(){
        string path = GetPath();
        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<LeaderboardData>(json);
        }
        else return new LeaderboardData();
    }
}