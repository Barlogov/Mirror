using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

[System.Serializable]
public class Session
{
    public string lobbyID;
    public List<Person> persons = new List<Person>();

    public Session(string lobbyId, Person person)
    {
        this.lobbyID = lobbyId;
        persons.Add(person);
    }
    
    public Session() { }
}

[System.Serializable]
public class SyncListGameObject : SyncList<GameObject> { }

[System.Serializable]
public class SyncListSession : SyncList<Session> { }

public class LobbyMaker : NetworkBehaviour
{
    public static LobbyMaker instance;
    
    public SyncListSession sessions = new SyncListSession();
    public SyncList<string> sessionIDs = new SyncList<string>();

    [SerializeField] GameObject dotsManagerPrefab;


    private void Start()
    {
        instance = this;
    }

    public bool HostSession(string _lobbyID, Person _person, out int personIndex)
    {
        personIndex = -1;
        if (!sessionIDs.Contains(_lobbyID))
        {
            sessionIDs.Add(_lobbyID);
            sessions.Add(new Session(_lobbyID, _person));
            Debug.Log($"Session generated");
            personIndex = 1;
            return true;
        }
        else
        {
            Debug.Log($"Session ID already exist");
            return false;
        }
        
    }
    
    public bool JoinSession(string _lobbyID, Person _person, out int personIndex)
    {
        personIndex = -1;
        if (sessionIDs.Contains(_lobbyID))
        {

            for (int i = 0; i < sessions.Count; i++)
            {
                if (sessions[i].lobbyID == _lobbyID)
                {
                    sessions[i].persons.Add(_person);
                    personIndex = sessions[i].persons.Count;
                    break;
                }
            }
            Debug.Log($"Session joined");
            return true;
        }
        else
        {
            Debug.Log($"Session ID does not exist");
            return false;
        }
        
    }

    public void BeginSession(string _lobbyID)
    {
        GameObject newDotsManager = Instantiate(dotsManagerPrefab);
        NetworkServer.Spawn(newDotsManager);
        newDotsManager.GetComponent<NetworkMatch>().matchId = _lobbyID.ToGuid();
        DotsManager dotsManager = newDotsManager.GetComponent<DotsManager>();

        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].lobbyID == _lobbyID)
            {
                foreach (var person in sessions[i].persons)
                {
                    Person _person = person.GetComponent<Person>();
                    dotsManager.AddPerson(_person);
                    _person.StartSession();
                }
                break;
            }
        }
    }
    
    public static string GetRandomLobbyID()
    {
        string _id = String.Empty;
        for (int i = 0; i < 5; i++)
        {
            int rand = Random.Range(0, 36);
            if (rand < 26)
            {
                _id += (char) (rand + 65);
            }
            else
            {
                _id += (rand - 26).ToString();
            }
        }
        Debug.Log($"ID: {_id}");
        return _id;
    }

    
}


public static class LobbyExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);
            
        return new Guid(hashBytes);
    }
}
