using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlock : NetworkBehaviour
{
    [SerializeField] private LayerMask bound;
    [SerializeField] private GameObject snowBlockPrefab;
    [SerializeField] private GameObject snowBlockFrame;

    public delegate void playerBlockDestroyedDelegate(SnowBlock p);

    private GameObject spawnedFrame;

    private float dist = 10f;
    private int snowCost = 3;

    private int maxBlocks = 8;
    [SyncVar]
    private int placedBlocks = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleFrame();
        }
        if (spawnedFrame != null) {
            MoveFrame();
            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                Debug.Log("Place Block");
                CmdPlace(GetComponent<NetworkIdentity>(), spawnedFrame.transform.position, spawnedFrame.transform.rotation);
            }
        }
    }

    [Command]
    private void CmdPlace(NetworkIdentity player, Vector3 framePos, Quaternion frameRot)
    {
        if (!(GetComponent<SnowGauge>().Has(snowCost) && placedBlocks < maxBlocks)) return;

        GetComponent<SnowGauge>().UseSnow(snowCost);
        GameObject block = (Instantiate(snowBlockPrefab, framePos + (Vector3.up * 1), frameRot));
        SnowBlock snowBlock = block.GetComponent<SnowBlock>();
        RandomRotate(block.transform);
        snowBlock.SetPlayer(player);
        snowBlock.BlockDestroyed += BlockDestroyed;
        placedBlocks++;
        NetworkServer.Spawn(block);
        RpcToggleFrame(GetComponent<NetworkIdentity>().connectionToServer);
    }

    private void BlockDestroyed(SnowBlock block)
    {
        placedBlocks--;
    }

    private void MoveFrame()
    {
        var direction = GetComponentInChildren<MouseLook>().GetDirection();
        var position = GetComponentInChildren<MouseLook>().GetPosition();
        if(Physics.Raycast(position, direction * Vector3.forward, out RaycastHit hit, dist)) {
            if (!((bound.value & (1 << hit.collider.gameObject.layer)) > 0)) {
                spawnedFrame.SetActive(true);
                spawnedFrame.transform.position = hit.point;
            } else
                spawnedFrame.SetActive(false);
        } else { 
            spawnedFrame.SetActive(false);
        }
    }

    private void RandomRotate(Transform block)
    {
        block.localRotation = Quaternion.Euler(Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90);
    }

    private void ToggleFrame()
    {
        if (spawnedFrame != null)
            Destroy(spawnedFrame);
        else {
            spawnedFrame = Instantiate(snowBlockFrame, this.transform);
            spawnedFrame.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    [TargetRpc]
    private void RpcToggleFrame(NetworkConnection conn)
    {
        ToggleFrame();
    }
}
