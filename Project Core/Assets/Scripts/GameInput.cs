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
    public event EventHandler OnCroutch;
    public event EventHandler OnstopCroutch;
    public event EventHandler OnShoot;
    public event EventHandler OnStopShoot;
    public event EventHandler OnShootAlternate;
    public event EventHandler OnStopShootAlternate;
    public event EventHandler OnSwitch;
    public event EventHandler OnSwitchAlternate;
    public void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Jump.performed += Jump_performed;
        playerInputAction.Player.Sprint.performed += Sprint_performed;
        playerInputAction.Player.Croutch.performed += Croutch_performed;
        playerInputAction.Player.Croutch.canceled += Croutch_canceled;
        playerInputAction.Player.Shoot.performed += Shoot_performed;
        playerInputAction.Player.Shoot.canceled += Shoot_canceled;
        playerInputAction.Player.ShootAlternate.performed += ShootAlternate_performed;
        playerInputAction.Player.ShootAlternate.canceled += ShootAlternate_canceled;
        playerInputAction.Player.Switch.performed += Switch_performed;
        playerInputAction.Player.SwitchAlternate.performed += SwitchAlternate_performed;
        
    }

    private void SwitchAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitchAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Switch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitch?.Invoke(this, EventArgs.Empty);
    }

    private void ShootAlternate_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStopShootAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void ShootAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStopShoot?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    private void Croutch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnstopCroutch?.Invoke(this, EventArgs.Empty);

    }

    private void Croutch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCroutch?.Invoke(this, EventArgs.Empty);
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
    

