using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeKinematic : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask block;

    private void OnCollisionEnter(Collision collision)
    {
        if ((ground.value & (1 << collision.gameObject.layer)) > 0 || (block.value & (1 << collision.gameObject.layer)) > 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
