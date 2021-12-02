using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLayer : NetworkBehaviour
{
    [SerializeField] private GameObject layerPrefab;
    [SerializeField] private Transform filling;

    private int maxLayers = 10;
    [SyncVar]
    private int currentLayers;

    [SyncVar]
    public NetworkIdentity parentIdentity;
    [SyncVar]
    private Vector3 pos;

    [Server]
    public void SetParentPos(NetworkIdentity identity, Vector3 pos)
    {
        parentIdentity = identity;
        this.pos = pos;
    }

    public override void OnStartClient()
    {
        transform.SetParent(parentIdentity.transform);
        transform.localPosition = pos;
    }

    void Start()
    {
        currentLayers = Random.Range(1, maxLayers + 1);
        var layer = Instantiate(layerPrefab, filling);
        layer.transform.localScale = Vector3.one / 2;
        layer.transform.localPosition = new Vector3(0, 0.35f, 0);
    }

    private void Update()
    {
        if (isServer) return;

        filling.transform.localScale = new Vector3(1, currentLayers, 1);
        filling.transform.localPosition = new Vector3(0, ((currentLayers - 1) * 0.03f), 0.05f);
    }

    [Server]
    public void AddSnow(int amount)
    {
        currentLayers += amount;
        if (currentLayers > maxLayers)
            currentLayers = maxLayers;
    }

    [Server]
    public void Take()
    {
        currentLayers--;
        if (currentLayers == 0)
            NetworkServer.Destroy(gameObject);
    }
}
