using UnityEngine;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(BaseMovment))]
public class ShipController : MonoBehaviour{
    private InputHandler inputHandler;
    private BaseMovment baseMovment;

    private void Awake(){
        inputHandler = GetComponent<InputHandler>();
        baseMovment = GetComponent<BaseMovment>();
    }
    private void OnEnable(){
        inputHandler.OnThrustChanged += SetThrust;
        inputHandler.OnRotationChanged += SetRotation;
    }
    private void OnDisable(){
        inputHandler.OnThrustChanged -= SetThrust;
        inputHandler.OnRotationChanged -= SetRotation;
    }

    private void SetThrust(float val) => baseMovment.SetForce(val);
    private void SetRotation(float val) => baseMovment.SetTorque(val);
}