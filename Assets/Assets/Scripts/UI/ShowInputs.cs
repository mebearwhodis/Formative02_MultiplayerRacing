using UnityEngine;
using UnityEngine.InputSystem;

public class ShowInputs : MonoBehaviour
{
    private VehiclesInputs _input;
    private SetPlayerProfile _playerProfile;
    [SerializeField] private GameObject _keyboardScheme;
    [SerializeField] private GameObject _controllerScheme;
    [SerializeField] private GameObject _ButtonY;
    [SerializeField] private GameObject _ButtonF;
    private bool _showInputs = false;
    private Camera _mainCamera;
    [SerializeField] private GameObject _disconnectedText;
    
    void Start()
    {
        _input = GetComponent<VehiclesInputs>();
        _playerProfile = GetComponent<SetPlayerProfile>();
        if (_playerProfile.PlayerInput.currentControlScheme == "Keyboard")
        {
            _ButtonY.SetActive(false);
            _ButtonF.SetActive(true);
        }
        else if (_playerProfile.PlayerInput.currentControlScheme == "Controller")
        {
            _ButtonY.SetActive(true);
            _ButtonF.SetActive(false);
        }
    }

    void Update()
    {
        if (_showInputs)
        {
            if (_playerProfile.PlayerInput.currentControlScheme == "Keyboard")
            {
                ShowCurrentInputs(0);
            }
            else if (_playerProfile.PlayerInput.currentControlScheme == "Controller")
            {
                ShowCurrentInputs(1);
            }
        }
        else
        {
            _keyboardScheme.SetActive(false);
            _controllerScheme.SetActive(false);
        }
    }

    public void OnShowInputs(InputValue value)
    {
        _showInputs = value.isPressed;
    }
    
    private void ShowCurrentInputs(int scheme)
    {
        switch (scheme)
        {
            case 0:
                _keyboardScheme.SetActive(true);
                _controllerScheme.SetActive(false);
                break;
            case 1:
                _keyboardScheme.SetActive(false);
                _controllerScheme.SetActive(true);
                break;
            default:
                _keyboardScheme.SetActive(false);
                _controllerScheme.SetActive(false);
                break;
        }
    }
}
