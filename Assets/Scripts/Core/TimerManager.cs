using UnityEngine;

public class TimerManager : MonoBehaviour{
    [SerializeField] private TimerData timerData;

    private void Start(){
        timerData.ResetTimer();
    }
    private void Update(){
        timerData.UpdateTimer(Time.deltaTime);
    }
}