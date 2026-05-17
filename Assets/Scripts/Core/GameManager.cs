using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameStates CurrentState {get; private set;}
    public event Action<GameStates> OnGameStateChanged;
    private PlayerInputActions _inputActions;
    public bool IsStartPlaying = false;
    private bool isOnPhone = false;
    private string userName;

    private void Awake(){
        // Standard Singleton Setup
        if (Instance == null) {
            Instance = this;
            _inputActions = new PlayerInputActions();
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        CurrentState = GameStates.MainMenu;
        Time.timeScale = 0f;
    }
    private void OnEnable(){
        if (Instance != this) return;
        if (_inputActions == null) _inputActions = new PlayerInputActions();

        _inputActions.Player.Enable();

        _inputActions.Player.Pause.performed -= TogglePause;
        _inputActions.Player.Pause.performed += TogglePause;
        ChangeState(GameStates.MainMenu);
    }
    private void OnDisable(){
        if (Instance != this) return;

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
    public void SetUserName(string userName){
        this.userName = userName;
    }
    public string GetUserName(){
        return userName;
    }
    public void SetOnPhone(bool onPhone){
        isOnPhone = onPhone;
    }
    public bool GetOnPhone(){
        return isOnPhone;
    }
    public void StartGame(){
        StartCoroutine(StartGameAfterItStarted());
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
    // Used when Player Dies
    public void GameOver(){
        TransitionManager.Instance.YouDiedTransition();
    }
    // Used when Player Wins
    public void WinGame(){
        TransitionManager.Instance.VictoryTransition();
    }
    public void QuitGame(){
        Application.Quit();
    }

    private IEnumerator StartGameAfterItStarted()
    {
        if(!IsStartPlaying) IsStartPlaying =true;
        else{
            RestartGame();
            yield return new WaitForSecondsRealtime(2);
        }
        ChangeState(GameStates.Playing);
    }

}