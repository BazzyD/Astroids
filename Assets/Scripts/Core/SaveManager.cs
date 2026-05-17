using UnityEngine;
using System.Collections.Generic;
using LootLocker.Requests; // Added for web backend

public class SaveManager : MonoBehaviour{
    public static SaveManager Instance;
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private TimerData timerData;
    private string leaderboardKey = "global_top_10"; // Match your LootLocker dashboard key
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
    private void Start()
    {
        // Connect anonymously to the web server as soon as the game boots up
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success) Debug.Log("Connected to Web Leaderboard Backend!");
            else Debug.LogError("Web Leaderboard Connection Failed: " + response.errorData.message);
        });
    }

    private void HandleGameStateChanged(GameStates gameState){
        if(gameState == GameStates.GameOver || gameState == GameStates.Win){
            SaveData();
        }
    }
    private void SaveData(){
        string pName = GameManager.Instance.GetUserName();
        int score = scoreData.currentScore;
        float time = timerData.currentTime;

        // 1. Tell the server the player's name
        LootLockerSDKManager.SetPlayerName(pName, (nameResponse) =>
        {
            if (!nameResponse.success) Debug.LogError("Failed to update player name on server");

            // Convert raw time float into a clean string (e.g., "01:45") to store in the metadata slot
            string formattedTime = FormatTime(time);

            // 2. Push the score and completion time string to the cloud
            LootLockerSDKManager.SubmitScore("", score, leaderboardKey, formattedTime, (scoreResponse) =>
            {
                if (scoreResponse.success) Debug.Log("Score successfully saved to the cloud!");
                else Debug.LogError("Failed to upload score: " + scoreResponse.errorData.message);
            });
        });
    }

    // Because web requests take time, we pass a callback function (System.Action) 
    // that triggers automatically once the data finishes downloading.
    public void LoadData(System.Action<LeaderboardData> onDataLoaded)
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            LeaderboardData webLeaderboard = new LeaderboardData();

            webLeaderboard.entries = new List<SaveEntry>();

            if (response.success)
            {
                if (response.items != null)
                {
                // Map the server data directly back into your existing SaveEntry format
                    foreach (var item in response.items)
                    {
                        SaveEntry entry = new SaveEntry
                        {
                            playerName = string.IsNullOrEmpty(item.player.name) ? $"Player {item.player.id}" : item.player.name,
                            score = item.score,
                            time = ParseFormattedTime(item.metadata) // Convert the metadata string back to a float for your system
                        };
                        webLeaderboard.entries.Add(entry);
                    }
                }
            }
            else
            {
                Debug.LogError("Could not fetch web leaderboard: " + response.errorData.message);
            }

            // Return the populated list back to whatever script asked for it (like your UI)
            onDataLoaded?.Invoke(webLeaderboard);
        });
    }
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Helper: Converts string ("01:35") back into a float for your TimerData system
    private float ParseFormattedTime(string formattedTime)
    {
        if (string.IsNullOrEmpty(formattedTime) || !formattedTime.Contains(":")) return 0f;
        string[] parts = formattedTime.Split(':');
        if (parts.Length != 2) return 0f;
        
        float.TryParse(parts[0], out float minutes);
        float.TryParse(parts[1], out float seconds);
        return (minutes * 60f) + seconds;
    }
}