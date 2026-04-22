using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameState CurrentState {get; private set;}
    public event Action<GameState> OnGameStateChanged;
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        // Standard Singleton Setup
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the manager alive between levels!
        } else {
            Destroy(gameObject);
            return;
        }
        CurrentState = GameState.MainMenu;
        Time.timeScale = 0f;
    }
    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Pause.performed += TogglePause;
    }
    private void OnDisable()
    {
        if (_inputActions != null)
        {
            _inputActions.Player.Pause.performed -= TogglePause;
            _inputActions.Player.Disable();
        }
    }
    private void TogglePause(InputAction.CallbackContext context) {
        if (CurrentState == GameState.Playing) PauseGame();
        else if (CurrentState == GameState.Pause) ResumeGame();
    }
    
    public void ChangeState(GameState newState)
    {
        if(CurrentState == newState) return;

        CurrentState = newState;

        Time.timeScale = (CurrentState == GameState.Playing) ? 1f : 0f;

        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeState(GameState.Playing);
    }
    public void PauseGame()
    {
        ChangeState(GameState.Pause);
    }
    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }
    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}