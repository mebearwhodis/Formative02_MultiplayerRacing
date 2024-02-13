using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V2_Online_ResetVehicle : MonoBehaviour
{
    private V2_Online_VehiclesInputs _input;
    private V2_Online_Checkpoints _checkpoints;
    private Rigidbody _rb;

    private void Start()
    {
        _input = GetComponent<V2_Online_VehiclesInputs>();
        _checkpoints = GetComponent<V2_Online_Checkpoints>();
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
        GameObject checkpoint = _checkpoints.CurrentCheckpoint;
        transform.position = checkpoint.transform.position;
        transform.rotation = checkpoint.transform.rotation;
        _rb.velocity = new Vector3(0,0,0);

    }
}