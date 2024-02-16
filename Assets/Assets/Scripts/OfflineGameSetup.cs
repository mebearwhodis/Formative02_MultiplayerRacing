using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OfflineGameSetup : MonoBehaviour
{
    
    [SerializeField] 
    private GameObject _vehiclePrefab;
    private List<SpawnPoints> _spawnPoints;
    [SerializeField] private TextMeshProUGUI _countdown;
    
    private void Start()
    {
        //Put all spawn points in a list
        _spawnPoints = GetComponentsInChildren<SpawnPoints>().ToList();
        
        //Put all players in a list
        List<OfflineLobbyPlayerSetup> _players = FindObjectsByType<OfflineLobbyPlayerSetup>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        
        //For each player, instantiate their vehicle
        for (int i = 0; i < _players.Count; i++)
        {
           GameObject newVehicle =  Instantiate(_vehiclePrefab,
                _spawnPoints[i % _spawnPoints.Count].transform.position,
                _spawnPoints[i % _spawnPoints.Count].transform.rotation
                );

           if (newVehicle.TryGetComponent<SetPlayerProfile>(out var shape))
           {
               shape.SetProfile(_players[i]);
           }
        }
        StartCoroutine(Countdown());
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player " + input.playerIndex + " joined");
    }
    
    private IEnumerator Countdown()
    {
        List<GameObject> _players = GameObject.FindGameObjectsWithTag("Player").ToList();
       
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].gameObject.GetComponent<PlayerInput>().actions.Disable();
        }
        
        int countdownValue = 5;

        // Countdown loop
        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownValue--;
            _countdown.text = countdownValue.ToString();
        }
        
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].gameObject.GetComponent<PlayerInput>().actions.Enable();
        }
        
        // Countdown reached 0, activate inputs
        _countdown.text = "Start!";
        yield return new WaitForSeconds(1f);
        _countdown.gameObject.SetActive(false);

    }
}