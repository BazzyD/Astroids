using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState gameState)
    {
        switch(gameState){
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            case GameState.Playing:
                ShowHUD();
                break;
            case GameState.Pause:
                ShowPauseMenu();
                break;
            case GameState.GameOver:
                ShowGameOverScreen();
                break;   
        }
    }

    private void HideAll()
    {
        mainMenuPanel.SetActive(false);
        hudPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    private void ShowMainMenu()
    {
        HideAll();
        mainMenuPanel.SetActive(true);
    }
    private void ShowHUD()
    {
        HideAll();
        hudPanel.SetActive(true);
    }
    private void ShowPauseMenu()
    {
        HideAll();
        pauseMenuPanel.SetActive(true);
    }
    private void ShowGameOverScreen()
    {
        HideAll();
        gameOverPanel.SetActive(true);
    }

    public void OnStartButtonClicked() {
        Debug.Log("Start Game");
        GameManager.Instance.StartGame();
    }

    public void OnRestartButtonClicked() {
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButtonClicked() {
        GameManager.Instance.QuitGame();
    }
}