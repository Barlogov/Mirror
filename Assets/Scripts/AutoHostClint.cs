using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoHostClint : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    
   

    public void JoinLocalHost()
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    }
}
