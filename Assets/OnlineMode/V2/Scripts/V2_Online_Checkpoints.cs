using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class V2_Online_Checkpoints : MonoBehaviour
{
    private Rigidbody _rb;
    private V2_Online_VehiclesInputs _input;
    private int _checkpointsPassed = 0;
    private GameObject _currentCheckpoint;
    [SerializeField]private bool _canEndTurn;
    private int _turnNumber = 0;

    public int TurnNumber => _turnNumber;
    public GameObject CurrentCheckpoint => _currentCheckpoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<V2_Online_VehiclesInputs>();
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
            //if (_checkpointsPassed == 10 && _canEndTurn)
            if (_canEndTurn)
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