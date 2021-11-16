using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSnow : NetworkBehaviour
{
    [SerializeField] private float pickUpRange = 10f;
    [SerializeField] private float basePickUpDelay = 0.5f;

    [SyncVar]
    private float elapsed;
    [SyncVar]
    private float pickUpDelay;

    public delegate void playerCollectSnowDelegate(CollectSnow p);
    public event playerCollectSnowDelegate PlayerCollectSnow;

    public void SetDelay(float pourcentage)
    {
        pickUpDelay = basePickUpDelay * pourcentage;
    }

    private void Start()
    {
        pickUpDelay = basePickUpDelay;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        elapsed += Time.deltaTime;
        if (elapsed >= pickUpDelay && Input.GetKeyDown(KeyCode.Mouse1)) {
            elapsed = 0;
            var look = GetComponentInChildren<MouseLook>();
            CmdPickUp(look.GetDirection(), look.GetPosition());
        }
    }

    [Command]
    private void CmdPickUp(Quaternion direction, Vector3 position)
    {
        //Debug.DrawRay(position, direction * Vector3.forward * pickUpRange, Color.green, 5);
        if (Physics.Raycast(position, direction * Vector3.forward, out RaycastHit hit, pickUpRange)) {
            //Debug.DrawRay(position, direction * Vector3.forward * hit.distance, Color.red, 5);
            SnowLayer layer = hit.transform.GetComponent<SnowLayer>();
            if (layer != null) {
                SnowGauge gauge = GetComponent<SnowGauge>();
                if (!gauge.IsFull()) {
                    layer.Take();
                    GetComponent<SnowGauge>().AddSnow(1);
                    RpcTakeSnow(GetComponent<NetworkIdentity>().connectionToServer);
                }
            }
        }
    }

    [TargetRpc]
    private void RpcTakeSnow(NetworkConnection conn)
    {
        PlayerCollectSnow?.Invoke(this);
    }
}
