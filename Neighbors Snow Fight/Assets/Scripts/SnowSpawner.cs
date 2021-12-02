using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject snowLayerPrefab;
    [SerializeField] private Transform start;
    [SerializeField] private NetworkIdentity startIdentity;

    void Start()
    {
        if (!isServerOnly) return;

        InitializeSnow();
    }

    private void InitializeSnow()
    {
        for (int i = 0; i < 44; i++) {
            for (int j = 0; j < 25; j++) {
                GameObject layer = Instantiate(snowLayerPrefab, start);
                Vector3 pos = new Vector3(i * -1.62f, 0, j * -1.62f);
                layer.GetComponent<SnowLayer>().SetParentPos(startIdentity, pos);
                layer.transform.localPosition = pos;
                NetworkServer.Spawn(layer);
            }
        }
    }
}
