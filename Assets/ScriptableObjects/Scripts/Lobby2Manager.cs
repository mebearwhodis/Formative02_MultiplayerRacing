using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Lobby2Manager : MonoBehaviour
{
    private List<SpawnPointGizmos> _spawnPoints = new List<SpawnPointGizmos>();

    private List<Lobby2Setup> _joinedSetups = new List<Lobby2Setup>();

    private void Start()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPointGizmos>().ToList();
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player Joined: "
                  + input.playerIndex + " : "
                  + input.gameObject.name + " : ** "
                  + input.currentControlScheme + " ** : ** "
                  + input.devices + " **" );

        if (input.gameObject.TryGetComponent<Lobby2Setup>(out var setup))
        {
            setup.transform.position = _spawnPoints[input.playerIndex].transform.position;
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
        foreach (Lobby2Setup setup in _joinedSetups)
        {
            if (!setup.Ready)
                everyoneIsReady = false;
        }

        if (everyoneIsReady)
        {
            foreach (Lobby2Setup setup in _joinedSetups)
            {
                setup.gameObject.SetActive(false);
            }
                

            SceneManager.LoadScene("GameTest");
        }
    }
}