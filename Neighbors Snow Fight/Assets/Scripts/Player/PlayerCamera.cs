using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;

    void Start()
    {
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        if (!identity.isLocalPlayer)
            Destroy(playerCamera);

    }

}
