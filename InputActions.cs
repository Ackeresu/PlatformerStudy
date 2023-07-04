using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InputActions : MonoBehaviour {

    private GameInput gameInput; // Get the info of the auto-generated class that manage the inputs
    
    public event EventHandler <Vector2> OnPlayerMovement;
    public event EventHandler <bool> OnPlayerRun;
    public event EventHandler OnPlayerJump;
    public event EventHandler OnPlayerDash;

    private void Awake() {
        gameInput = new GameInput();

        gameInput.Player.Move.started += Move;
        gameInput.Player.Move.canceled += Move;
        gameInput.Player.Move.performed += Move;

        gameInput.Player.Run.started += Run;
        gameInput.Player.Run.canceled += Run;
        gameInput.Player.Run.performed += Run;

        gameInput.Player.Jump.performed += JumpPerformed;
        gameInput.Player.Dash.performed += DashPerformed;

        gameInput.Player.Enable();
    }

    private void Move(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Vector2 movement = gameInput.Player.Move.ReadValue<Vector2>();
        OnPlayerMovement?.Invoke(this, movement);
    }

    private void Run(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        bool running = gameInput.Player.Run.IsPressed();
        OnPlayerRun?.Invoke(this, running);
    }

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerJump?.Invoke(this, EventArgs.Empty);
    }

    private void DashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }
}
