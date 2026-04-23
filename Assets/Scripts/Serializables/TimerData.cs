using UnityEngine;

[CreateAssetMenu(fileName = "NewTimerData", menuName = "GameData/TimerData")]
public class TimerData : ScriptableObject
{
    public float currentTime = 0f;

    public System.Action<float> OnTimerChanged;

    public void UpdateTimer(float amount)
    {
        currentTime += amount;
        OnTimerChanged?.Invoke(currentTime);
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        OnTimerChanged?.Invoke(currentTime);
    }
}