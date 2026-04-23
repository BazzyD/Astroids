using UnityEngine;

[CreateAssetMenu(fileName = "NewScoreData", menuName = "GameData/ScoreData")]
public class ScoreData : ScriptableObject
{
    public int currentScore =0;

    public System.Action<int> OnScoreChanged;

    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }
}