using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetVehicle : MonoBehaviour
{
    private VehiclesInputs _input;
    private Checkpoints _checkpoints;
    private Rigidbody _rb;

    private void Start()
    {
        _input = GetComponent<VehiclesInputs>();
        _checkpoints = GetComponentInChildren<Checkpoints>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_input._positionReset)
        {
            resetVehiclePos();
        }
    }
    
    private void resetVehiclePos()
    {
        transform.position = _checkpoints.RespawnPoint;
        transform.rotation = _checkpoints.Rotation;
        _rb.velocity = new Vector3(0,0,0);

    }
}