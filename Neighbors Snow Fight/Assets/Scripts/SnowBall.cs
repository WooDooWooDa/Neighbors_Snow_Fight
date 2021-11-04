using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    [SerializeField] private LayerMask snowBlockLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask ground;

    [SerializeField] private int hitPoint = 1;

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
        if ((ground.value & (1 << collision.gameObject.layer)) > 0) {
            Debug.Log("Snow Ball hit ground");
        } else if ((playerLayer.value & 1 << collision.gameObject.layer) > 0 && collision.gameObject.GetComponent<PlayerShoot>() != launcher) {
            //knock player
            Debug.Log("Snow Ball hit player");
        } else if ((snowBlockLayer.value & 1 << collision.gameObject.layer) > 0) {
            Debug.Log("Snow Ball hit snow block");
            collision.gameObject.GetComponent<SnowBlock>().DamageBlock(hitPoint);
        }
    }
}
