using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    [SerializeField] private LayerMask snowBlockLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask ground;

    [SerializeField] private int hitPoint = 1;


    void Start()
    {


        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ground) {

        } else if (collision.gameObject.layer == playerLayer) {
            //knock player
        } else if (collision.gameObject.layer == snowBlockLayer) {
            collision.gameObject.GetComponent<SnowBlock>().DamageBlock(hitPoint);
        }
    }
}
