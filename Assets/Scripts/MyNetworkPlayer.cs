using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeReference] private TMP_Text _displayNameText = null;
    //[SerializeReference] private TMP_Vertex _displayColourRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))] [SerializeField]
    private string _displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColourUpdated))] [SerializeField]
    private Color _displayColour = Color.black;

    #region Server

    [Server] //Not needed but a good security
    public void SetDisplayName(string newDisplayName)
    {
        _displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        _displayColour = newDisplayColour;
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

    #endregion

    #region Client

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        _displayNameText.text = newName;
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

    #endregion
}