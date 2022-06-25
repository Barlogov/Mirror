using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Person : NetworkBehaviour
{

    public static Person localPerson;
    [SyncVar]public string lobbyID;
    [SyncVar] public int personIndex;
    
    NetworkMatch networkMatch;

    private void Start()
    {
        networkMatch = GetComponent<NetworkMatch>();
        
        if (isLocalPlayer)
        {
            localPerson = this;
        }
        else
        {
            UILobby.instance.SpawnPersonUIPrefab(this);
        }
    }

    /*
     * HOST SESSION
     */
    public void HostSession()
    {
        string lobbyID = LobbyMaker.GetRandomLobbyID();
        CmdHostSession(lobbyID);
    }

    [Command]
    void CmdHostSession(string _lobbyID)
    {
        lobbyID = _lobbyID;
        if (LobbyMaker.instance.HostSession(_lobbyID, this, out personIndex))
        {
            Debug.Log($"Hosted Successfully");
            networkMatch.matchId = _lobbyID.ToGuid();
            TargetHostLobby(true, _lobbyID, personIndex);
        }
        else
        {
            Debug.Log($"Hosted Unsuccessfully");
            TargetHostLobby(false, _lobbyID, personIndex);
        }
    }
    
    
    [TargetRpc]
    void TargetHostLobby (bool success, string _lobbyID, int _personIndex) {
        personIndex = _personIndex;
        lobbyID = _lobbyID;
        Debug.Log($"Lobby ID: {lobbyID} == {_lobbyID}");
        UILobby.instance.HostSuccess(success, _lobbyID);
    }
    
     
    /*
     * JOIN SESSION
     */
    
    public void JoinSession(string _inputID)
    {
        CmdJoinSession(_inputID);
    }

    [Command]
    void CmdJoinSession(string _lobbyID)
    {
        lobbyID = _lobbyID;
        if (LobbyMaker.instance.JoinSession(_lobbyID, this, out personIndex))
        {
            Debug.Log($"Joined Successfully");
            networkMatch.matchId = _lobbyID.ToGuid();
            TargetJoinLobby(true, _lobbyID, personIndex);
        }
        else
        {
            Debug.Log($"Joined Unsuccessfully");
            TargetJoinLobby(false, _lobbyID, personIndex);
        }
    }
    
    [TargetRpc]
    void TargetJoinLobby (bool success, string _lobbyID, int _personIndex)
    {
        personIndex = _personIndex;
        lobbyID = _lobbyID;
        Debug.Log($"Lobby ID: {lobbyID} == {_lobbyID}");
        UILobby.instance.JoinSuccess(success, lobbyID);
    }
    /*
     * BEGIN SESSION
     */
    
    public void BeginSession()
    {
        CmdBeginSession();
    }

    [Command]
    void CmdBeginSession()
    {
        LobbyMaker.instance.BeginSession(lobbyID);
        Debug.Log($"Session started");
        
    }

    public void StartSession()
    {
        TargetBeginLobby();
    }
    
    
    [TargetRpc]
    void TargetBeginLobby () {
        Debug.Log($"Lobby ID: {lobbyID} | Beginning");
        // Additivly load scene
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
    
}
