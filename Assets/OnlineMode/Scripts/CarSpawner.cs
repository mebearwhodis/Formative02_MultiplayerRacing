using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CarSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject _playerCharacter = null;
    [SerializeField] private Transform _playerCharacterSpawnPoint = null;
    
    #region Server

    [Command]
    private void CmdSpawnCharacter()
    {
        GameObject playerCharacterInstance = Instantiate(_playerCharacter, _playerCharacterSpawnPoint.position,
            _playerCharacterSpawnPoint.rotation);
        
        NetworkServer.Spawn(playerCharacterInstance, connectionToClient);
    }
    
    #endregion
    
    #region Client
    
    
    
    #endregion
}
