using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   private PlayerInputActions playerInputActions;
   public Action<float> OnThrustChanged;
   public Action<float> OnRotationChanged;
   public Action<bool> OnFirePerformed;
   public Action OnExplodePerformed;

   
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
      playerInputActions.Player.Shoot.canceled += OnShoot;
   }
   private void OnDisable() {
      playerInputActions.Player.Disable();

      playerInputActions.Player.Move.performed -= OnThrust;
      playerInputActions.Player.Move.canceled -= OnThrust;

      playerInputActions.Player.Rotation.performed -= OnRotation;
      playerInputActions.Player.Rotation.canceled -= OnRotation;

      playerInputActions.Player.Shoot.performed -= OnShoot;
      playerInputActions.Player.Shoot.canceled -= OnShoot;
   }

   private void Update()
   {
      var keyboard = Keyboard.current;
      if (keyboard == null) return;

      if (keyboard.eKey.wasPressedThisFrame)
      {
         OnExplodePerformed?.Invoke();
      }
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
      bool isPressed = context.ReadValueAsButton();
      OnFirePerformed?.Invoke(isPressed);
   }

}