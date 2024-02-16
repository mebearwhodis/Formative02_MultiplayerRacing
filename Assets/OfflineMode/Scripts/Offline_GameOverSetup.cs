
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Offline_GameOverSetup : MonoBehaviour
{
    
    [SerializeField] 
    private GameObject _vehiclePrefab;
    private List<SpawnPoints> _spawnPoints;
    
    private void Start()
    {
        //Put all spawn points in a list
        _spawnPoints = GetComponentsInChildren<SpawnPoints>().ToList();
        
        //Put all players in a list
        List<OfflineLobbyPlayerSetup> _players = FindObjectsByType<OfflineLobbyPlayerSetup>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        _players.OrderByDescending(player => player.GetComponent<SetPlayerProfile>().IsFirst).ToList();
        
        //For each player, instantiate their vehicle
        for (int i = 0; i < _players.Count; i++)
        {
            GameObject newVehicle =  Instantiate(_players[i].gameObject,
                _spawnPoints[i % _spawnPoints.Count].transform.position,
                _spawnPoints[i % _spawnPoints.Count].transform.rotation
            );
            
            if (newVehicle.TryGetComponent<SetPlayerProfile>(out var shape))
            {
                shape.SetGameOverProfile(_players[i]);
            }
        }
    }
}