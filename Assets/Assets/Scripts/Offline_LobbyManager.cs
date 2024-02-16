using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Offline_LobbyManager : MonoBehaviour
{
    private List<SpawnPoints> _spawnPoints = new List<SpawnPoints>();

    private List<OfflineLobbyPlayerSetup> _joinedSetups = new List<OfflineLobbyPlayerSetup>();

    private void Start()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoints>().ToList();
        //Tried to destroy the prefabs in "DontDestroyOnLoad" to allow for a quick restart of the game but it didn't work, try something else
        // List<OfflineLobbyPlayerSetup> _players = FindObjectsByType<OfflineLobbyPlayerSetup>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        // for (int i = 0; i < _players.Count; i++)
        // {
        //     Destroy(_players[i].transform.parent);
        // }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player Joined: "
                  + input.playerIndex + " : "
                  + input.gameObject.name + " : ** "
                  + input.currentControlScheme + " ** : ** "
                  + input.devices + " **" );

        if (input.gameObject.TryGetComponent<OfflineLobbyPlayerSetup>(out var setup))
        {
            setup.transform.position = _spawnPoints[input.playerIndex].transform.position;
            setup.transform.rotation = _spawnPoints[input.playerIndex].transform.rotation;
            setup.BindInputs(input);

            setup.OnReady += CheckEveryoneIsReady;
            _joinedSetups.Add(setup);

            DontDestroyOnLoad(setup);
        }
    }

    private void CheckEveryoneIsReady()
    {
        if (_joinedSetups.Count < 2)
            return;
        bool everyoneIsReady = true;
        foreach (OfflineLobbyPlayerSetup setup in _joinedSetups)
        {
            if (!setup.Ready)
                everyoneIsReady = false;
        }

        if (everyoneIsReady)
        {
            foreach (OfflineLobbyPlayerSetup setup in _joinedSetups)
            {
                setup.gameObject.SetActive(false);
            }
                

            SceneManager.LoadScene("LevelOne");
        }
    }
}