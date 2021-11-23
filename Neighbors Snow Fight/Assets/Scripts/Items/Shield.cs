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
        transform.SetParent(parentIdentity.transform);
    }
}
