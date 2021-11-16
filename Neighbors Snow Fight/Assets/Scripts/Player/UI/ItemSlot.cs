using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ItemSlot : NetworkBehaviour
{
    [SerializeField] private Transform modelParent;
    [SerializeField] private TextMeshProUGUI timeLeft;

    private BaseItem item;

    public void Initiate(BaseItem item)
    {
        this.item = item;
        SpawnModel();
    }

    private void SpawnModel()
    {
        var model = Instantiate(item.GetModel(), modelParent);
        model.transform.localScale = new Vector3(25, 25, 1);
    }

    void Update()
    {
        if (!isServer) return;

        RpcUpdateTimeLeft(GetComponentInParent<NetworkIdentity>().connectionToServer, item.GetTimeLeft());
    }

    [TargetRpc]
    private void RpcUpdateTimeLeft(NetworkConnection conn, float time)
    {
        timeLeft.text = time.ToString("0") + " sec";
    }
}
