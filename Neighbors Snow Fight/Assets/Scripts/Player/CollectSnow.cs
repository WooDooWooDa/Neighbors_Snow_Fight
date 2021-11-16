using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSnow : NetworkBehaviour
{
    [SerializeField] private float pickUpRange = 10f;
    [SerializeField] private float basePickUpDelay = 0.5f;

    private float elapsed;
    private float pickUpDelay;

    public delegate void playerCollectSnowDelegate(CollectSnow p);
    public event playerCollectSnowDelegate PlayerCollectSnow;

    [Server]
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
        if (isServerOnly)
            elapsed += Time.deltaTime;

        if (!isLocalPlayer) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Debug.Log("Pick up snow");
            var look = GetComponentInChildren<MouseLook>();
            CmdPickUp(look.GetDirection(), look.GetPosition());
        }
    }

    [Command]
    private void CmdPickUp(Quaternion direction, Vector3 position)
    {
        if (!(elapsed >= pickUpDelay)) return;
        elapsed = 0;
        //Debug.DrawRay(position, direction * Vector3.forward * pickUpRange, Color.green, 5);
        if (Physics.Raycast(position, direction * Vector3.forward, out RaycastHit hit, pickUpRange)) {
            //Debug.DrawRay(position, direction * Vector3.forward * hit.distance, Color.red, 5);
            SnowLayer layer = hit.transform.GetComponent<SnowLayer>();
            if (layer != null) {
                SnowGauge gauge = GetComponent<SnowGauge>();
                if (!gauge.IsFull()) {
                    layer.Take();
                    GetComponent<SnowGauge>().AddSnow(1);
                    PlayerCollectSnow?.Invoke(this);
                }
            }
        }
    }

}
