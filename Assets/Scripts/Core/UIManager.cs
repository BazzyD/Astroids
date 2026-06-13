using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour{
    public static UIManager Instance;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject StartNewGamePanel;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TMP_InputField playerNameinput;
    [SerializeField] private Toggle myToggle;
    [SerializeField] private Button StartNewGameButton;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private LeaderboardDisplay leaderboard;

    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject touchInputs;
    [SerializeField] private TextMeshProUGUI nextLevelDisplayer;
    [SerializeField] private GameObject pauseMenuPanel;
    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject WinPanel;
    private Coroutine _levelCoroutine;
    private string userName;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip toggleSound;
    [SerializeField] private AudioClip nextLevelSound;


    private void Awake(){
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        nextLevelDisplayer.enabled =false;
    }
    private void Start(){
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
    private void OnEnable(){
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        PressureManager.Instance.OnChangeLevel += HandleLevelChanged;
        if (myToggle != null)
        {
            // Clean up any old listeners first to prevent double-triggering
            myToggle.onValueChanged.RemoveListener(OnToggleChanged);
            
            // Dynamically assign the method when the scene loads/enables
            myToggle.onValueChanged.AddListener(OnToggleChanged);
            myToggle.isOn = GameManager.Instance.GetOnPhone(); // Set initial state based on GameManager's value
        }
    }
    private void OnDisable(){
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        PressureManager.Instance.OnChangeLevel -= HandleLevelChanged;
        // Unsubscribe when leaving the scene to prevent memory leaks
        myToggle?.onValueChanged.RemoveListener(OnToggleChanged);
    }
    private void OnToggleChanged(bool isOn)
    {
        AudioManager.Instance.PlaySFX(toggleSound);
        GameManager.Instance.SetOnPhone(isOn); 
    }
    private void HandleGameStateChanged(GameStates gameState){
        HideAll();
        switch(gameState){
            case GameStates.MainMenu:
                AudioManager.Instance.PlayMenuBGMusic();
                mainMenuPanel.SetActive(true);
                break;
            case GameStates.NewGameMenu:
                StartNewGamePanel.SetActive(true);
                StartNewGameButton.interactable = false;
                break;
            case GameStates.Playing:
                AudioManager.Instance.PlayGameBGMusic();
                hudPanel.SetActive(true);
                touchInputs.SetActive(GameManager.Instance.GetOnPhone());
                break;
            case GameStates.Pause:
                //AudioManager.Instance.PlayMenuBGMusic();
                pauseMenuPanel.SetActive(true);
                break;
            case GameStates.Leaderboard:
                AudioManager.Instance.PlayMenuBGMusic();
                leaderboardPanel.SetActive(true);
                leaderboard.DisplayLeaderboard();
                break;
            case GameStates.GameOver:
                AudioManager.Instance.PlayGameOverBGMusic();
                gameOverPanel.SetActive(true);
                break;
            case GameStates.Win:
                AudioManager.Instance.PlayWinBGMusic();
                WinPanel.SetActive(true);
                break;
        }
    }
    private void HandleLevelChanged(int level)
    {
        if (_levelCoroutine != null) StopCoroutine(_levelCoroutine);
        _levelCoroutine = StartCoroutine(GoingToNextLevel(level));
    }
    private IEnumerator GoingToNextLevel(int level)
    {
        if(level<=1) yield break; // No need to show for level 1

        AudioManager.Instance.PlayAlarm(nextLevelSound);
        float duration =5f;
        float flickerInterval = 0.2f;
        nextLevelDisplayer.text = $"Going Into Level {level}";
        while(duration >0){
            nextLevelDisplayer.enabled = !nextLevelDisplayer.enabled;
            yield return new WaitForSeconds(flickerInterval);
            duration -= flickerInterval;
        }
        nextLevelDisplayer.enabled = false;
        AudioManager.Instance.StopAlarm();
    }
    private void HideAll(){
        StartNewGameButton.interactable = false;
        StartNewGamePanel.SetActive(false);
        WinPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        hudPanel.SetActive(false);
        touchInputs.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    public void OnStartButtonClicked() {
        ButtonClickSound();
        GameManager.Instance.SetUserName(userName);
        GameManager.Instance.StartGame();
    }
    public void OnStartNewGameButtonClicked() {
        HideAll();
        ButtonClickSound();
        playerNameText.text = "";
        playerNameinput.text ="";
        GameManager.Instance.SetUserName("");
        GameManager.Instance.ChangeState(GameStates.NewGameMenu);
    }
    public void AllowStartNewGame(string currentText) {
        if(currentText.Length >= 3)
            StartNewGameButton.interactable = true;
        else
            StartNewGameButton.interactable = false;
        userName = currentText;
    }
    public void OnRestartButtonClicked() {
        ButtonClickSound();
        GameManager.Instance.RestartGame();
    }
    public void OnResumeButtonClicked(){
        ButtonClickSound();
        GameManager.Instance.ChangeState(GameStates.Playing);
    }
    public void OnQuitButtonClicked()
    {
        ButtonClickSound();
        GameManager.Instance.QuitGame();
    }
    public void OnMainMenuButtonClicked() {
        ButtonClickSound();
        GameManager.Instance.ChangeState(GameStates.MainMenu);
    }
    public void OnLeaderboardButtonClicked()
    {
        ButtonClickSound();
        GameManager.Instance.ChangeState(GameStates.Leaderboard);
    }

    private void ButtonClickSound()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
    }
}