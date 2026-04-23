using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    private void OnEnable()
    {
        scoreData.OnScoreChanged += UpdateScoreDisplay;
    }
    private void OnDisable()
    {
        scoreData.OnScoreChanged -= UpdateScoreDisplay;
    }
    private void Start()
    {
        UpdateScoreDisplay(0);
    }
    private void UpdateScoreDisplay(int score){
        scoreText.text = $"{score}";
    }
    
}
