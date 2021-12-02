using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerExt : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection player)
    {
        base.OnServerAddPlayer(player);
        Debug.LogWarning("Player connected");
    }
}
