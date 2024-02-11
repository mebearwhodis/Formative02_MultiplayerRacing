using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class OfflineLobbyPlayerSetup : MonoBehaviour
{
    [SerializeField] private List<VehicleProfile> _profiles;
    [SerializeField] private GameObject _body;

    public Action OnReady;
    
    private int _playerIndex;
    private string _controlScheme;
    private InputDevice[] _devices;
    

    private int _modelIndex = 0;
    private bool _ready;

    public bool Ready => _ready;
    public VehicleProfile Profile => _profiles[_modelIndex];
    public int PlayerIndex => _playerIndex;
    public string ControlScheme => _controlScheme;
    public InputDevice[] Devices => _devices;

    private void Start()
    {
        LoadNewModel();
    }

    void OnChangeModel(InputValue value)
    {
        float v = value.Get<float>();

        if (v == 0f) return;
        if (Mathf.Abs(v) == 1)
        {
            _modelIndex += Mathf.FloorToInt(v);
            if (_modelIndex < 0)
                _modelIndex = _profiles.Count - 1;
            else if (_modelIndex >= _profiles.Count)
                _modelIndex = 0;
        }

        LoadNewModel();
    }

    void OnConfirm(InputValue input)
    {
        _ready = true;
        OnReady?.Invoke();
    }

    void OnCancel(InputValue input)
    {
        _ready = false;
    }
    
    public void BindInputs(PlayerInput input)
    {
        _playerIndex = input.playerIndex;
        _controlScheme = input.currentControlScheme;
        _devices = input.devices.ToArray();
    }

    private void LoadNewModel()
    {
        Destroy(_body);
        _body = Instantiate(_profiles[_modelIndex]._model, transform);
    }
}
