using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class V2_Online_VehiclesInputs : NetworkBehaviour
{
    [Header("Vehicle Input Values")] 
    [SyncVar] public float _accelerationValue = 0;
    [SyncVar] public float _steerValue = 0;
    [SyncVar] public float _brakeValue = 0;
    [SyncVar] public bool _itemUsed = false;
    [SyncVar] public bool _positionReset = false;
    [SyncVar] public bool _isConfused = false;

    // Ensure correct ownership
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        // Enable input actions
        EnableInput();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();

        // Disable input actions
        DisableInput();
    }

    private void EnableInput()
    {
        // Enable input actions
        InputSystem.onDeviceChange += OnDeviceChange;
        InputActionAsset asset = Resources.Load<InputActionAsset>("CarControl");
        if (asset != null)
        {
            asset.Enable();
        }
    }

    private void DisableInput()
    {
        // Disable input actions
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputActionAsset asset = Resources.Load<InputActionAsset>("CarControl");
        if (asset != null)
        {
            asset.Disable();
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected && device == InputSystem.devices[0])
        {
            // Re-enable input when the device is reconnected
            EnableInput();
        }
    }

    [Command]
    private void CmdSetAcceleration(float value)
    {
        _accelerationValue = value;
    }

    [Command]
    private void CmdSetSteer(float value)
    {
        _steerValue = !_isConfused ? value : -1f * value;
    }

    [Command]
    private void CmdSetBrake(float value)
    {
        _brakeValue = value;
    }

    [Command]
    private void CmdSetItemUsed(bool value)
    {
        _itemUsed = value;
    }

    [Command]
    private void CmdSetPositionReset(bool value)
    {
        _positionReset = value;
    }

    [Command]
    private void CmdSetConfused(bool value)
    {
        _isConfused = value;
    }

    public void OnAccelerate(InputValue value)
    {
        if (!isOwned) return;
        CmdSetAcceleration(value.Get<float>());
    }

    public void OnSteer(InputValue value)
    {
        if (!isOwned) return;
        CmdSetSteer(value.Get<float>());
    }

    public void OnBrake(InputValue value)
    {
        if (!isOwned) return;
        CmdSetBrake(value.Get<float>());
    }

    public void OnUse(InputValue value)
    {
        if (!isOwned) return;
        CmdSetItemUsed(value.isPressed);
    }

    public void OnReset(InputValue value)
    {
        if (!isOwned) return;
        CmdSetPositionReset(value.isPressed);
    }
}
