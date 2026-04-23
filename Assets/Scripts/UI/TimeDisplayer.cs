using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    [SerializeField] private TimerData timerData;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    private void OnEnable()
    {
        timerData.OnTimerChanged += UpdateTimerDisplay;
    }
    private void OnDisable()
    {
        timerData.OnTimerChanged -= UpdateTimerDisplay;
    }
    private void Start()
    {
        UpdateTimerDisplay(0);
    }
    private void UpdateTimerDisplay(float timer){
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        // This creates a string like "02:05"
        string timeString = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        
        timerText.text = timeString;
    }
    
}
