using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner2 : MonoBehaviour
{

    [SerializeField]private GameObject _vehiclePrefab;
    private List<SpawnPointGizmos> _spawnPoints;
    
    private void Start()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPointGizmos>().ToList();
        
        List<Lobby2Setup> _setups = FindObjectsByType<Lobby2Setup>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        
        for (int i = 0; i < _setups.Count; i++)
        {
           GameObject newVehicle =  Instantiate(_vehiclePrefab,
                _spawnPoints[i % _spawnPoints.Count].transform.position,
                _spawnPoints[i % _spawnPoints.Count].transform.rotation
                );

           if (newVehicle.TryGetComponent<VehicleShape>(out var shape))
           {
               shape.SetProfile(_setups[i]);
           }
        }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player " + input.playerIndex + " joined");
    }
}
