using UnityEngine;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    private VehiclesInputs _input;

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
        //public GameObject wheelEffectObj;
        //public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    private Rigidbody carRb;


    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        _input = GetComponent<VehiclesInputs>();

    }

    void Update()
    {
        AnimateWheels();
        //WheelEffects();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = _input._accelerationValue * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = _input._steerValue * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (_input._brakeValue >= 0.1f || _input._accelerationValue == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }

        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }

        }
    }

    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void UseItem()
    {
        if (_input._itemUsed)
        {
            Debug.Log("Using item");
        }
    }
    
    void ResetPosition()
    {
        if (_input._positionReset)
        {
            Debug.Log("Resetting position");
        }
    }

    // void WheelEffects()
    // {
    //     foreach (var wheel in wheels)
    //     {
    //         //var dirtParticleMainSettings = wheel.smokeParticle.main;
    //
    //         if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
    //         {
    //             wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
    //             wheel.smokeParticle.Emit(1);
    //         }
    //         else
    //         {
    //             wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
    //         }
    //     }
    // }
}