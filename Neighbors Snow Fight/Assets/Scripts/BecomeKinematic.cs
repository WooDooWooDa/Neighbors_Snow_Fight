using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeKinematic : MonoBehaviour
{
    [SerializeField] private LayerMask ground;

    private void OnCollisionEnter(Collision collision)
    {
        if ((ground.value & (1 << collision.gameObject.layer)) > 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
