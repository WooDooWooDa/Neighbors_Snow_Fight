using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerExt : NetworkManager
{
    private void OnPlayerConnected(NetworkIdentity player)
    {
        Debug.Log("Player connected");
    }

    private void OnPlayerDisconnected(NetworkIdentity player)
    {
        Debug.Log("Player disconnected");
    }
}
