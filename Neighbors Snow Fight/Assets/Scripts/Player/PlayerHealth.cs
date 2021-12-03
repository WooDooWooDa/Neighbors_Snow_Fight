using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private Transform shieldParent;
    [SerializeField] private Transform UI;
    [SerializeField] private GameObject screenSnow;

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
            StartCoroutine(Knock());
        }
    }

    [Server]
    private IEnumerator Knock()
    {
        GetComponent<PlayerMouvement>().SetSpeed(0.1f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerMouvement>().SetSpeed(1f);
        RpcKnock(GetComponent<NetworkIdentity>());
    }

    [TargetRpc]
    private void RpcKnock(NetworkIdentity conn)
    {
        StartCoroutine(ScreenSnow());
    }

    private IEnumerator ScreenSnow()
    {
        var snow = Instantiate(screenSnow, UI);
        snow.transform.Rotate(new Vector3(0, 0, Random.Range(-2, 2)));
        yield return new WaitForSeconds(1f);
        Destroy(snow, 5);
        for (var i = 1; i <= 3; i++) {
            snow.transform.Translate(Vector3.down * i * 100);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    [ClientRpc]
    private void RpcShield(NetworkIdentity identity, GameObject shield)
    {
        if (GetComponent<NetworkIdentity>() == identity) {
            Destroy(shield); //Destroying shield on right player client
        }
    }

}
