using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;
    
    [Header("Host Join")]
    [SerializeField] InputField joinSessionID;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;
    [SerializeField] Canvas lobbyCanvas;

    [Header("Lobby")] 
    [SerializeField] Transform UIPersonParent;
    [SerializeField] GameObject UIPersonPrefab;
    [SerializeField] Text lobbyIDText;
    [SerializeField] GameObject beginSessionButton;
    void Start () {
        instance = this;
    }

    /*
     * HOST SESSION
     */
    
    public void Host()
    {
        joinSessionID.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
        
        Person.localPerson.HostSession();
    }

    public void HostSuccess(bool success, string lobbyID)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            
            SpawnPersonUIPrefab(Person.localPerson);
            lobbyIDText.text = lobbyID;
            beginSessionButton.SetActive(true);
        }
        else
        {   
            joinSessionID.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }
    
    /*
     * JOIN SESSION
     */
    
    public void Join()
    {
        joinSessionID.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
        
        Person.localPerson.JoinSession(joinSessionID.text.ToUpper());
    }
    
    public void JoinSuccess(bool success, string lobbyID)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            
            SpawnPersonUIPrefab(Person.localPerson);
            lobbyIDText.text = lobbyID;
        }
        else
        {   
            joinSessionID.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }
    
    

    public void SpawnPersonUIPrefab(Person person)
    {
        GameObject newUIPerson = Instantiate(UIPersonPrefab, UIPersonParent);
        newUIPerson.GetComponent<UIPerson>().SetPerson(person);
        newUIPerson.transform.SetSiblingIndex(person.personIndex-1);
    }

    public void BeginSession()
    {
        Person.localPerson.BeginSession();
    }
}
