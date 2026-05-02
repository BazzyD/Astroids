using UnityEngine;

public class Boosters : MonoBehaviour
{
    [SerializeField] private GameObject topBoosters;
    [SerializeField] private GameObject bottomBoosters;
    [SerializeField] private GameObject turnRightBoosters;
    [SerializeField] private GameObject turnLeftBoosters;
    private InputHandler _inputHandler;
    private void Awake() {
        _inputHandler = GetComponentInParent<InputHandler>();
        
    }
    private void OnEnable() {
        _inputHandler.OnThrustChanged += OnThrustChanged;
        _inputHandler.OnRotationChanged += OnRotationChanged;
    }
    private void OnDisable() {
        _inputHandler.OnThrustChanged -= OnThrustChanged;
        _inputHandler.OnRotationChanged -= OnRotationChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        topBoosters.SetActive(false);
        bottomBoosters.SetActive(false);
        turnRightBoosters.SetActive(false);
        turnLeftBoosters.SetActive(false);
    }

    private void OnThrustChanged(float thrustValue)
    {
        if(thrustValue > 0f)
        {
            bottomBoosters.SetActive(true);
            topBoosters.SetActive(false);
        }
        else if(thrustValue < 0f)
        {
            bottomBoosters.SetActive(false);
            topBoosters.SetActive(true);
        }
        else
        {
            bottomBoosters.SetActive(false);
            topBoosters.SetActive(false);
        }
    }
    private void OnRotationChanged(float rotationValue)
    {
       if(rotationValue > 0f)
        {
            turnRightBoosters.SetActive(true);
            turnLeftBoosters.SetActive(false);
        }
        else if(rotationValue < 0f)
        {
            turnRightBoosters.SetActive(false);
            turnLeftBoosters.SetActive(true);
        }
        else
        {
            turnRightBoosters.SetActive(false);
            turnLeftBoosters.SetActive(false);
        }
    }
}
