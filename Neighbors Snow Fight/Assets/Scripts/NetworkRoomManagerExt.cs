using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRoomManagerExt : NetworkRoomManager
{
    public override void OnRoomClientEnter() {
        Debug.Log($"enter client index : {clientIndex}");
    }

    public override void OnRoomClientExit()
    {
        Debug.Log($"exit client index : {clientIndex}");
    }
}
