using UnityEngine;
public class UIManager : MonoBehaviour{
    public static UIManager Instance;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private LeaderboardDisplay leaderboard;
    [SerializeField] private GameObject gameOverPanel;
    private void Awake(){
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }
    private void Start(){
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
    private void OnEnable(){
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }
    private void OnDisable(){
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }
    private void HandleGameStateChanged(GameStates gameState){
        HideAll();
        switch(gameState){
            case GameStates.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameStates.Playing:
                hudPanel.SetActive(true);
                break;
            case GameStates.Pause:
                pauseMenuPanel.SetActive(true);
                break;
            case GameStates.Leaderboard:
                leaderboardPanel.SetActive(true);
                leaderboard.DisplayLeaderboard();
                break;
            case GameStates.GameOver:
                gameOverPanel.SetActive(true);
                break;
        }
    }
    private void HideAll(){
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        hudPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    public void OnStartButtonClicked() => GameManager.Instance.StartGame();
    public void OnRestartButtonClicked() => GameManager.Instance.RestartGame();
    public void OnQuitButtonClicked() => GameManager.Instance.QuitGame();
    public void OnMainMenuButtonClicked() {
        Debug.Log("Main Menu Button Clicked");
        GameManager.Instance.ChangeState(GameStates.MainMenu);
    }
    public void OnLeaderboardButtonClicked()
    {
        Debug.Log("Leaderboard Button Clicked");
        GameManager.Instance.ChangeState(GameStates.Leaderboard);
    }
        

}