using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   private PlayerInputActions playerInputActions;
   public Action<float> OnThrustChanged;
   public Action<float> OnRotationChanged;
   public Action<Vector2> OnMovmentChange; 
   public Action<bool> OnFirePerformed;
   public Action OnExplodePerformed;

   
   private void Awake(){
      playerInputActions = new PlayerInputActions();
   }

   private void OnEnable() {
      playerInputActions.Player.Enable();

      playerInputActions.Player.Move.performed += OnNavigate;
      playerInputActions.Player.Move.canceled += OnNavigate;

      playerInputActions.Player.Shoot.performed += OnShoot;
      playerInputActions.Player.Shoot.canceled += OnShoot;
   }
   private void OnDisable() {
      playerInputActions.Player.Disable();

      playerInputActions.Player.Move.performed -= OnNavigate;
      playerInputActions.Player.Move.canceled -= OnNavigate;

      playerInputActions.Player.Shoot.performed -= OnShoot;
      playerInputActions.Player.Shoot.canceled -= OnShoot;
   }

  
   private void OnNavigate(InputAction.CallbackContext context)
   {
      if(Time.timeScale == 0f) return;

      // Read the 2D position of the joystick or WASD keys
      Vector2 navigationInput = context.ReadValue<Vector2>();
      if(GameManager.Instance.GetOnPhone()){
         OnMovmentChange?.Invoke(navigationInput);
         return;
      }
      
      // 1. The Y axis represents forward/backward push (Thrust)
      OnThrustChanged?.Invoke(navigationInput.y);

      // 2. The X axis represents left/right push (Rotation)
      OnRotationChanged?.Invoke(navigationInput.x);
   }
   private void OnShoot(InputAction.CallbackContext context)
   {
      if(Time.timeScale == 0f) return;
      bool isPressed = context.ReadValueAsButton();
      OnFirePerformed?.Invoke(isPressed);
   }

}