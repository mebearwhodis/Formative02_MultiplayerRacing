using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyUI = null;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_Text[] _playerNamesTexts = new TMP_Text[4];

    private void Start()
    {
        MyNetworkManager.ClientOnConnected += HandleClientConnected;
        MyNetworkPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        MyNetworkPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    private void OnDestroy()
    {
        MyNetworkManager.ClientOnConnected -= HandleClientConnected;
        MyNetworkPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        MyNetworkPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void HandleClientConnected()
    {
        _lobbyUI.SetActive(true);
    }
    
    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        _startGameButton.gameObject.SetActive(state);
    }

    private void ClientHandleInfoUpdated()
    {
        List<MyNetworkPlayer> players = ((MyNetworkManager)NetworkManager.singleton)._players;

        for (int i = 0; i < players.Count; i++)
        {
            _playerNamesTexts[i].text = players[i].GetDisplayName();
        }

        for (int i = players.Count; i < _playerNamesTexts.Length; i++)
        {
            _playerNamesTexts[i].text = "Waiting For Player...";
        }

        _startGameButton.interactable = players.Count >= 2;
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<MyNetworkPlayer>().CmdStartGame();
    }

    public void LeaveLobby()
    {
        //If you're a host (server active on your machine & connected as client)
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();

            //Returns to the Main Menu, with everything enabled/disabled - just make sure scene 0 is the main menu
            SceneManager.LoadScene(0);
        }
    }
}