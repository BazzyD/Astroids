using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   private PlayerInputActions playerInputActions;

   private void Awake(){
      playerInputActions = new PlayerInputActions();
   }

   private void OnEnable() {
      playerInputActions.Player.Enable();
   }
   private void OnDisable() {
      playerInputActions.Player.Disable();
   }
   private void Update()
   {

   }
   
   public Vector2 GetMovementVectorNormalized()
   {
      Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
      return inputVector.normalized;
   }

}