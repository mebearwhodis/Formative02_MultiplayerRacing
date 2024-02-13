using System;
using System.Collections;
using System.Collections.Generic;
using Ashsvp;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class V2_Online_SetPlayerProfile : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<LayerMask> _playerLayers;
    [SerializeField] private V2_Online_Speedometer _speedometer;
    [SerializeField] private V2_Online_LapCounter _lapCounter;

    private VehicleProfile _profile;

    //public void SetProfile(OfflineLobbyPlayerSetup setup)
    //{
        //_profile = setup.Profile;
        
        //Shapeshifting
        // Destroy(_body);
        // _body = Instantiate(_profile._prefab, transform);
   private void Start()
   {
       
        //Inputs re-binding
        //_playerInput.SwitchCurrentControlScheme(setup.ControlScheme, setup.Devices);
        
        //Assign camera follow & lookat
        CinemachineVirtualCamera virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = _body.transform;
        virtualCamera.LookAt = _body.transform;
        
        //Convert layer mask from bit to integer
        //int layerToAdd = (int)Mathf.Log(_playerLayers[setup.PlayerIndex].value, 2);
        
        //Set the layer
        // gameObject.layer = layerToAdd;
        // virtualCamera.gameObject.layer = layerToAdd;
        
        //Add the layer to the mask using bitwise operation
        // GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

        _speedometer.target = _body.GetComponent<Rigidbody>();
        _lapCounter._target = _body;

    }
    
}
