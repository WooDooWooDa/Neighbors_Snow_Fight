using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowDrop : NetworkBehaviour
{
    [SerializeField] private LayerMask snowLayerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;

        if (HitThisLayer(snowLayerLayer, collision)) {
            collision.gameObject.GetComponent<SnowLayer>().AddSnow(1);
        }
        NetworkServer.Destroy(gameObject);
    }

    private bool HitThisLayer(LayerMask layer, Collision collision)
    {
        return (layer.value & 1 << collision.gameObject.layer) > 0;
    }
}
