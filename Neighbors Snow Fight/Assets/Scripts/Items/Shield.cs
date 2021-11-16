using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : NetworkBehaviour
{
    [SyncVar]
    public NetworkIdentity parentIdentity;

    public override void OnStartClient()
    {
        GameObject parentObject = null; //change
        transform.SetParent(parentObject.transform);
    }
}
