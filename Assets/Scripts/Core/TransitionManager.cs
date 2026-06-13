using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    [SerializeField] private CanvasGroup faderCanvasGroup; 
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private CanvasGroup victoryCanvasGroup; 
    [SerializeField] private Animator victoryAnimator;
    [SerializeField] private CanvasGroup youDiedCanvasGroup; 
    [SerializeField] private Animator youDiedAnimator;
    
    [Header("Audio")]
    [SerializeField] private AudioClip fadeinSound;
    [SerializeField] private AudioClip fadeOutSound;


    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadLevelAsync(int levelIndex)
    {
        if(GameManager.Instance.IsStartPlaying){
            faderAnimator.SetTrigger("FadeIn");
            AudioManager.Instance.PlaySFX(fadeinSound);
            yield return new WaitForSecondsRealtime(1f);
            faderCanvasGroup.blocksRaycasts = true; 
        }

        // Start loading in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        // Wait until the scene is fully loaded
        while (!operation.isDone)
        {
            //Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null; 
        }
        if(GameManager.Instance.IsStartPlaying){
            // Wait a tiny bit more for everything to initialize
            yield return new WaitForSecondsRealtime(0.5f);

            // 5. Hide the fader
            faderAnimator.SetTrigger("FadeOut");
            AudioManager.Instance.PlaySFX(fadeOutSound);
            faderCanvasGroup.blocksRaycasts = false;
            GameManager.Instance.ChangeState(GameStates.Playing);
        }
    }
    public void VictoryTransition()
    {
        StartCoroutine(TransitionRoutine(victoryAnimator,victoryCanvasGroup,GameStates.Win));
    }
    public void YouDiedTransition()
    {
        StartCoroutine(TransitionRoutine(youDiedAnimator,youDiedCanvasGroup,GameStates.GameOver));
    }
    private IEnumerator TransitionRoutine(Animator ani, CanvasGroup cg, GameStates state)
    {
        Time.timeScale = 0f;
        ani.SetTrigger("FadeIn");
        AudioManager.Instance.PlaySFX(fadeinSound);
        cg.blocksRaycasts = true; 
        yield return new WaitForSecondsRealtime(2f);
        ani.SetTrigger("FadeOut");
        AudioManager.Instance.PlaySFX(fadeOutSound);
        cg.blocksRaycasts = false;
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.ChangeState(state);
    }
}