using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Offline_GameOverSetup : MonoBehaviour
{
    
    [SerializeField] 
    private GameObject _vehiclePrefab;
    private List<SpawnPoints> _spawnPoints;
    [SerializeField] private TextMeshProUGUI _countdown;

    [SerializeField] private PlayerInputManager _playerInputManager;
    
    private void Start()
    {
        // _playerInputManager.splitScreen = false;
        // //Put all spawn points in a list
        // _spawnPoints = GetComponentsInChildren<SpawnPoints>().ToList();
        //
        // //Put all players in a list
        // List<OfflineLobbyPlayerSetup> _players = FindObjectsByType<OfflineLobbyPlayerSetup>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        //
        // //For each player, instantiate their vehicle
        // for (int i = 0; i < _players.Count; i++)
        // {
        //    GameObject newVehicle =  Instantiate(_vehiclePrefab,
        //         _spawnPoints[i % _spawnPoints.Count].transform.position,
        //         _spawnPoints[i % _spawnPoints.Count].transform.rotation
        //         );
        //
        //    if (newVehicle.TryGetComponent<SetPlayerProfile>(out var shape))
        //    {
        //        shape.SetProfile(_players[i]);
        //    }
        // }
        StartCoroutine(Countdown());
    }
    
    private IEnumerator Countdown()
    {
        //List<GameObject> _players = GameObject.FindGameObjectsWithTag("Player").ToList();
       
        // for (int i = 0; i < _players.Count; i++)
        // {
        //     _players[i].gameObject.GetComponent<PlayerInput>().actions.Disable();
        //     _players[i].gameObject.GetComponentInChildren<Rigidbody>().useGravity = false;
        // }
        
        int countdownValue = 10;

        // Countdown loop
        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownValue--;
            //_countdown.text = "Going Back to Lobby in " + countdownValue;
            _countdown.text = "Quitting the game in " + countdownValue ;
        }
        
        // Countdown reached 0, going back to the lobby
        //SceneManager.LoadScene("Lobby");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}