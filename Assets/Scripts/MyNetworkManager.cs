using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _playerCar = null;
    
    //OnClient already exists in the Network Manager
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool _isGameON = false;
    //{get;} makes it so we can only get it form elsewhere, not set it
    public List<MyNetworkPlayer> _players { get; } = new List<MyNetworkPlayer>();
    
    #region Server

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        //Kick players if game is in progress, aka don't allow them to join an ongoing game
        if(!_isGameON){return;}
        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

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
        ServerChangeScene("SampleScene");
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        _players.Add(player);
        
        player.SetDisplayName($"Player {numPlayers}");
        player.SetDisplayColour(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        
        //Sets the first person to join (so, the host) as party owner, only them can start the game
        player.SetPartyOwner(_players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Sample"))
        {
            //     GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            //     NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
            foreach (MyNetworkPlayer player in _players)
            {
                GameObject baseInstante = Instantiate(_playerCar, GetStartPosition().position, quaternion.identity);
            
                NetworkServer.Spawn(baseInstante, player.connectionToClient);
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
