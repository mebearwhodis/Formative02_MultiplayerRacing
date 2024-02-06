using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class VehiclesInputs : MonoBehaviour
{
    [Header("Vehicle Input Values")] 
    
    public float _accelerationValue = 0;
    public float _steerValue = 0;
    public float _brakeValue = 0;
    
    public bool _itemUsed = false;
    public bool _positionReset = false;

        public void OnAccelerate(InputValue value)
        {
            _accelerationValue = value.Get<float>();
        }
        
        public void OnSteer(InputValue value)
        {
            _steerValue = value.Get<float>();
        }

        public void OnBrake(InputValue value)
        {
            _brakeValue = value.Get<float>();
        }

        public void OnUse(InputValue value)
        {
            _itemUsed = value.isPressed;
        }

        public void OnReset(InputValue value)
        {
            _positionReset = value.isPressed;
        }
}