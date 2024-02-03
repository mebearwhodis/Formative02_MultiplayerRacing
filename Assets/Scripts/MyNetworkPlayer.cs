using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeReference] private TMP_Text _displayNameText = null;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))] [SerializeField]
    private string _displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColourUpdated))] [SerializeField]
    private Color _displayColour = Color.black;

    public static event Action ClientOnInfoUpdated;
    

    [SyncVar(hook =nameof(AuthorityHandlePartyOwnerStateUpdated))] private bool _isPartyOwner = false;

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    
    public bool GetIsPartyOwner()
    {
        return _isPartyOwner;
    }

    public string GetDisplayName()
    {
        return _displayName;
    }
    
    #region Server

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    [Server] //Not needed but a good security
    public void SetDisplayName(string newDisplayName)
    {
        this._displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        _displayColour = newDisplayColour;
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        _isPartyOwner = state;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        //Server validation, check if name is valid
        if(newDisplayName.Length is < 2 or > 10)
        {return;}
        
        RpcLogNewName(newDisplayName);
        
        SetDisplayName(newDisplayName);
    }

    [Command]
    public void CmdStartGame()
    {
        if(!_isPartyOwner){return;}
        ((MyNetworkManager)NetworkManager.singleton).StartGame();
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if(NetworkServer.active){return;}
        
        DontDestroyOnLoad(gameObject);
        
        ((MyNetworkManager)NetworkManager.singleton)._players.Add(this);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();
        if(!isClientOnly){return;}
        ((MyNetworkManager)NetworkManager.singleton)._players.Remove(this);
        
        //if(!isOwned){return;} ... other things. He changed the first if
    }

    private void ClientHandleDisplayNameUpdated(string oldName, string newName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    private void HandleDisplayColourUpdated(Color oldColour, Color newColour)
    {
        _displayNameText.color = newColour;
    }

    [ContextMenu("Set Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("Bobby");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if(!isOwned){return;}

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    #endregion
}