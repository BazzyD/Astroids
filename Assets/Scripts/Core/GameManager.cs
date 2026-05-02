using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameStates CurrentState {get; private set;}
    public event Action<GameStates> OnGameStateChanged;
    private PlayerInputActions _inputActions;

    private void Awake(){
        // Standard Singleton Setup
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        CurrentState = GameStates.MainMenu;
        Time.timeScale = 0f;
    }
    private void OnEnable(){
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Pause.performed += TogglePause;
    }
    private void OnDisable(){
        if (_inputActions != null){
            _inputActions.Player.Pause.performed -= TogglePause;
            _inputActions.Player.Disable();
        }
    }
    private void TogglePause(InputAction.CallbackContext context) {
        if (CurrentState == GameStates.Playing) PauseGame();
        else if (CurrentState == GameStates.Pause) ResumeGame();
    }
    public void ChangeState(GameStates newState)
    {
        if(CurrentState == newState) return;

        CurrentState = newState;

        Time.timeScale = (CurrentState == GameStates.Playing) ? 1f : 0f;

        OnGameStateChanged?.Invoke(CurrentState);
    }
    public void StartGame(){
        ChangeState(GameStates.Playing);
    }
    public void RestartGame(){
        TransitionManager.Instance.RestartGame();
    }
    public void PauseGame(){
        ChangeState(GameStates.Pause);
    }
    public void ResumeGame(){
        ChangeState(GameStates.Playing);
    }
    public void EndGame(){
        ChangeState(GameStates.GameOver);
    }
    public void QuitGame(){
        Application.Quit();
    }
}