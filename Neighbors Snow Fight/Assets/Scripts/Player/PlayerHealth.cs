using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private Transform shieldParent;

    public delegate void playerHitDelegate(PlayerHealth p, int hitPoint);
    public event playerHitDelegate PlayerHit;

    private bool hasShield = false;
    private GameObject currentShield = null;

    public void HasShield(bool value, GameObject shield = null)
    {
        hasShield = value;
        if (value) {
            currentShield = Instantiate(shield, shieldParent.position, Quaternion.identity, shieldParent);
            currentShield.GetComponent<Shield>().parentIdentity = this.GetComponent<NetworkIdentity>();
            NetworkServer.Spawn(currentShield);
            RpcShield(this.GetComponent<NetworkIdentity>(), currentShield);
            return;
        }
        NetworkServer.Destroy(currentShield);
    }

    public void Hit(int hitPoint)
    {
        PlayerHit?.Invoke(this, hitPoint);

        if (!hasShield) {
            Debug.Log("Knock");
        }
    }

    [ClientRpc]
    private void RpcShield(NetworkIdentity identity, GameObject shield)
    {
        if (GetComponent<NetworkIdentity>() == identity) {
            Destroy(shield);
            Debug.Log("Destroying shield on right player client");
        }
    }

}
