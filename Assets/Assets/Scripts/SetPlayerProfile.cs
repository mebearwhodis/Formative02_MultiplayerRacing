using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetPlayerProfile : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<LayerMask> _playerLayers;
    [SerializeField] private Speedometer _speedometer;
    [SerializeField] private LapCounter _lapCounter;
    [SerializeField] private GameObject _disconnectedText;
    private bool _isPaused = false;
    
    private VehicleProfile _profile;

    public VehicleProfile Profile => _profile;

    public PlayerInput PlayerInput => _playerInput;



    public void SetProfile(OfflineLobbyPlayerSetup setup)
    {
        _profile = setup.Profile;
        
        //Shapeshifting
        Destroy(_body);
        _body = Instantiate(_profile._prefab, transform);
        
        //Inputs re-binding
        _playerInput.SwitchCurrentControlScheme(setup.ControlScheme, setup.Devices);
        
        //Assign camera follow & lookat
        CinemachineVirtualCamera virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = _body.transform;
        virtualCamera.LookAt = _body.transform;
        
        //Convert layer mask from bit to integer
        int layerToAdd = (int)Mathf.Log(_playerLayers[setup.PlayerIndex].value, 2);
        
        //Set the layer
        gameObject.layer = layerToAdd;
        virtualCamera.gameObject.layer = layerToAdd;
        
        //Add the layer to the mask using bitwise operation
        GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

        _speedometer.target = _body.GetComponent<Rigidbody>();
        _lapCounter._target = _body;

        
        _disconnectedText.gameObject.SetActive(false);
        _playerInput.onDeviceLost += OnDeviceLost;
        _playerInput.onDeviceRegained += OnDeviceRegained;
    }
        
    private void OnDeviceLost(PlayerInput input)
    {
        PauseGame();
    }
    
    private void OnDeviceRegained(PlayerInput input)
    {
        UnpauseGame();
    }

    public void SetGameOverProfile(OfflineLobbyPlayerSetup setup)
    {
        _profile = setup.Profile;
        
        //Shapeshifting
        Destroy(_body);
        _body = Instantiate(_profile._prefab, transform);
    }
    
    private void PauseGame()
    {
        if (!_isPaused)
        {
            _disconnectedText.gameObject.SetActive(true);
            Time.timeScale = 0f;
            _isPaused = true;
        }
    }

    private void UnpauseGame()
    {
        if (_isPaused)
        {
            _disconnectedText.gameObject.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }
    }
}
