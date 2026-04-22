using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   private PlayerInputActions playerInputActions;
   public Action<float> OnThrustChanged;
   public Action<float> OnRotationChanged;
   public Action OnFirePerformed;
   private void Awake(){
      playerInputActions = new PlayerInputActions();
   }

   private void OnEnable() {
      playerInputActions.Player.Enable();

      playerInputActions.Player.Move.performed += OnThrust;
      playerInputActions.Player.Move.canceled += OnThrust;

      playerInputActions.Player.Rotation.performed += OnRotation;
      playerInputActions.Player.Rotation.canceled += OnRotation;

      playerInputActions.Player.Shoot.performed += OnShoot;
   }
   private void OnDisable() {
      playerInputActions.Player.Disable();

      playerInputActions.Player.Move.performed -= OnThrust;
      playerInputActions.Player.Move.canceled -= OnThrust;

      playerInputActions.Player.Rotation.performed -= OnRotation;
      playerInputActions.Player.Rotation.canceled -= OnRotation;

      playerInputActions.Player.Shoot.performed -= OnShoot;
   }   

   private void OnThrust(InputAction.CallbackContext context)
   {
      float trustValue =  context.ReadValue<float>();;
      OnThrustChanged?.Invoke(trustValue);
   }
   private void OnRotation(InputAction.CallbackContext context)
   {
      float rotationValue =  context.ReadValue<float>();
      OnRotationChanged?.Invoke(rotationValue);
   }
   private void OnShoot(InputAction.CallbackContext context)
   {
      OnFirePerformed?.Invoke();
   }

}