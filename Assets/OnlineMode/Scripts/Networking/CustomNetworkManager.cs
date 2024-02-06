using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _playerCharacter = null;
    
    //OnClient already exists in the Network Manager
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool _isGameON = false;
    //{get;} makes it so we can only get it form elsewhere, not set it
    public List<CustomNetworkPlayer> _players { get; } = new List<CustomNetworkPlayer>();
    
    #region Server

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        //Kick players if game is in progress, aka don't allow them to join an ongoing game
        if(!_isGameON){return;}
        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        CustomNetworkPlayer player = conn.identity.GetComponent<CustomNetworkPlayer>();

        _players.Remove(player);
        
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        _players.Clear();
        _isGameON = false;
    }

    public void StartGame()
    {
        if(_players.Count <2){return;}

        _isGameON = true;
        //Loads the specified scene
        ServerChangeScene("OnlineLevel");
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //Do the base stuff, aka spawn the empty game object that is the player
        base.OnServerAddPlayer(conn);
        
        CustomNetworkPlayer player = conn.identity.GetComponent<CustomNetworkPlayer>();
        _players.Add(player);
        
        player.SetDisplayName($"Player {numPlayers}");
        player.SetDisplayColour(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        
        //Sets the first person to join (so, the host) as party owner, only them can start the game
        player.SetPartyOwner(_players.Count == 1);
    }
    
    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("OnlineLevel"))
        {
            foreach (CustomNetworkPlayer player in _players)
            {

                
                //V1
                GameObject playerCharacterInstance = Instantiate(_playerCharacter, GetStartPosition().position, quaternion.identity);
                
                NetworkServer.Spawn(playerCharacterInstance, player.connectionToClient);
                
                //V2
                // GameObject playerCharacterInstance = Instantiate(_playerCharacter, player.netIdentity.transform.position, player.netIdentity.transform.rotation);
                //
                // NetworkServer.Spawn(playerCharacterInstance, player.connectionToClient);
                
                //V3
                // Transform spawnPoint = player.transform;
                //
                // GameObject playerCharacterInstance = Instantiate(_playerCharacter, spawnPoint.position, spawnPoint.rotation);
                //
                // NetworkServer.Spawn(playerCharacterInstance, player.connectionToClient);
            }
        }
    }

    

    #endregion
    
    #region Client
    
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        ClientOnConnected?.Invoke();
        Debug.Log("Connected to a server!");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        
        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        _players.Clear();
    }

    #endregion

}
