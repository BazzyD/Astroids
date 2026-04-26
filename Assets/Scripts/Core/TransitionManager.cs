using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    [SerializeField] private CanvasGroup faderCanvasGroup; 
    [SerializeField] private Animator faderAnimator;


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
        faderAnimator.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        faderCanvasGroup.blocksRaycasts = true; 

        // Start loading in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        // Wait until the scene is fully loaded
        while (!operation.isDone)
        {
            //Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null; 
        }
        // Wait a tiny bit more for everything to initialize
        yield return new WaitForSecondsRealtime(0.5f);

        // 5. Hide the fader
        faderAnimator.SetTrigger("FadeOut");
        faderCanvasGroup.blocksRaycasts = false;
        GameManager.Instance.ChangeState(GameState.Playing);
    }
}