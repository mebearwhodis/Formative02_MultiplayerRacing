using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleShape : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private PlayerInput _playerInput;

    private LobbyProfile _profile;

    public void SetProfile(Lobby2Setup setup)
    {
        _profile = setup.Profile;
        
        //Shapeshifting
        Destroy(_body);
        _body = Instantiate(_profile._prefab, transform);
        
        //Inputs re-binding
        _playerInput.SwitchCurrentControlScheme(setup.ControlScheme, setup.Devices);
    }
    
}
