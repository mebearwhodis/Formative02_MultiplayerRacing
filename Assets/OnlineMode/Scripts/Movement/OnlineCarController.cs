using UnityEngine;
using System;
using System.Collections.Generic;
using Mirror;

public class OnlineCarController : NetworkBehaviour
{
    
    private Camera _mainCamera;
    
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    //private CarLights carLights;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        //carLights = GetComponent<CarLights>();
    }

    // void Update()
    // {
    //     GetInputs();
    //     // AnimateWheels();
    //     // WheelEffects();
    // }
    //
    // void LateUpdate()
    // {
    //     Move();
    //     Steer();
    //     Brake();
    // }
    
    void GetInputs()
    {
        if(control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }
    
    #region Server
    
    [Command]
    private void CmdMove(float input)
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = input * 600 * maxAcceleration * Time.deltaTime;
        }
    }
    
    [Command]
    private void CmdSteer(float input)
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = input * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    
    #endregion
    
    #region Client

    public override void OnStartAuthority()
    {
        _mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isOwned) {return;}
        GetInputs();
        CmdMove(moveInput);
        CmdSteer(steerInput);
    }
    #endregion
}