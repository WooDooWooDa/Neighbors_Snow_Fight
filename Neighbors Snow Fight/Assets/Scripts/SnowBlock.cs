using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : NetworkBehaviour
{
    [SerializeField] List<GameObject> snowBlockStates;
    [SerializeField] Transform parent;

    public delegate void playerBlockDestroyedDelegate(SnowBlock p);
    public event playerBlockDestroyedDelegate BlockDestroyed;

    private NetworkIdentity player;

    private int maxHitPoint = 4;
    private int hitPoint = 0;

    void Start()
    {
        SpawnBlocks();
    }

    private void Update()
    {
        if (!isServerOnly) return;

        if (hitPoint >= maxHitPoint) {
            RpcBlockDestroyed(player.GetComponent<NetworkIdentity>().connectionToServer); // RPC cant be called because conn is null ???
            NetworkServer.Destroy(parent.gameObject);
        }
    }

    public bool BelongsTo(PlayerShoot launcher)
    {
        return player == launcher.GetComponent<NetworkIdentity>();
    }

    [Server]
    public void SetPlayer(NetworkIdentity player)
    {
        this.player = player;
    }

    [Server]
    public bool DamageBlock(int dmg)
    {
        NetworkServer.Destroy(parent.GetChild(0).gameObject);
        hitPoint += dmg;
        if (hitPoint >= maxHitPoint) {
            return true;
        } else {
            SpawnBlocks();
            RpcUpdateBlockState(hitPoint);
            return false;
        }
    }

    private void SpawnBlocks()
    {
        var block = Instantiate(snowBlockStates[hitPoint], parent);
        block.transform.localRotation = Quaternion.identity;
        if (hitPoint == 0)
        {
            block.transform.localPosition = Vector3.zero;
        } else
        {
            block.transform.localPosition = new Vector3(0, -0.8f, 0.1f);
            block.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    [ClientRpc]
    private void RpcUpdateBlockState(int hitPoint)
    {
        Destroy(parent.GetChild(0).gameObject);
        var block = Instantiate(snowBlockStates[hitPoint], parent);
        block.transform.localRotation = Quaternion.identity;
        if (hitPoint == 0)
        {
            block.transform.localPosition = Vector3.zero;
        } else
        {
            block.transform.localPosition = new Vector3(0, -0.8f, 0.1f);
            block.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    [TargetRpc]
    private void RpcBlockDestroyed(NetworkConnection conn)
    {
        Debug.Log("block destroyed");
        BlockDestroyed?.Invoke(this);
    }
}
