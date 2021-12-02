using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : NetworkBehaviour
{
    [SerializeField] private int hitPoint = 1;
    [SerializeField] private GameObject snowLayer;

    [Header("Hit Layers")]
    [SerializeField] private LayerMask snowBlockLayer;
    [SerializeField] private LayerMask snowLayerLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask ground;

    private PlayerShoot launcher;

    public void SetLauncher(PlayerShoot playerShoot)
    {
        launcher = playerShoot;
    }

    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;

        if (collision.gameObject.GetComponent<PlayerShoot>() == launcher) return;

        if (HitThisLayer(ground, collision)) {
            HitGround(collision);
        } else if (HitThisLayer(playerLayer, collision)) {
            HitPlayer(collision);
            launcher.GetComponent<Score>().AddPoints(hitPoint);
        } else if (HitThisLayer(snowBlockLayer, collision)) {
            HitBlock(collision);
        } else if (HitThisLayer(snowLayerLayer, collision)) {
            HitLayer(collision);
        }
    }

    private bool HitThisLayer(LayerMask layer, Collision collision)
    {
        return (layer.value & 1 << collision.gameObject.layer) > 0;
    }

    private void HitGround(Collision collision)
    {
        if (UnityEngine.Random.Range(0, 100) < 20)
            NetworkServer.Spawn(Instantiate(snowLayer, transform.position, Quaternion.identity));
        NetworkServer.Destroy(gameObject);
    }

    private void HitPlayer(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>().Hit(hitPoint);
        NetworkServer.Destroy(gameObject);
    }

    private void HitBlock(Collision collision)
    {
        SnowBlock snowBlock = collision.gameObject.GetComponent<SnowBlock>();
        if (snowBlock.DamageBlock(hitPoint) && !snowBlock.BelongsTo(launcher))
            launcher.GetComponent<Score>().AddPoints(1);
    }

    private void HitLayer(Collision collision)
    {
        if (UnityEngine.Random.Range(0, 100) < (20 * hitPoint))
            collision.gameObject.GetComponent<SnowLayer>().AddSnow(1);
        NetworkServer.Destroy(gameObject);
    }

}
