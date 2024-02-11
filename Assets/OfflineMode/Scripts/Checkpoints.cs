using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private Rigidbody _rb;
    private VehiclesInputs _input;
    private int _checkpointsPassed = 0;
    private GameObject _currentCheckpoint;
    private bool _canEndTurn;
    private int _turnNumber = 0;

    public GameObject CurrentCheckpoint => _currentCheckpoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<VehiclesInputs>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            if (_currentCheckpoint != other.gameObject)
            {
                _checkpointsPassed++;
            }
            _currentCheckpoint = other.gameObject;
        }
        else if (other.gameObject.CompareTag("MidCheckpoint"))
        {
            _canEndTurn = true;
            _currentCheckpoint = other.gameObject;
        }
        else if (other.gameObject.CompareTag("FinishLine"))
        {
            if (_checkpointsPassed == 10 && _canEndTurn)
            {
                if (_turnNumber == 3)
                {
                    //End Game
                }
                else
                {
                    _canEndTurn = false;
                    _checkpointsPassed = 0;
                    _turnNumber++;
                }
            }

            _currentCheckpoint = other.gameObject;
        }
    }
}