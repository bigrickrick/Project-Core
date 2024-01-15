using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }
    private PlayerInputAction playerInputAction;
    public event EventHandler Onjump;
    public event EventHandler OnStartSprint;
    public event EventHandler OnStopSprint;
    public event EventHandler OnCroutch;
    public event EventHandler OnstopCroutch;
    public void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Jump.performed += Jump_performed;
        playerInputAction.Player.Sprint.performed += Sprint_performed;
        playerInputAction.Player.Sprint.canceled += Sprint_canceled;
        playerInputAction.Player.Croutch.performed += Croutch_performed;
        playerInputAction.Player.Croutch.canceled += Croutch_canceled;
        
    }

    private void Croutch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnstopCroutch?.Invoke(this, EventArgs.Empty);

    }

    private void Croutch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCroutch?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStopSprint?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStartSprint?.Invoke(this, EventArgs.Empty);

    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Onjump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();


        inputVector = inputVector.normalized;


        return inputVector;
    }
}
    

