using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Transform shieldParent;

    public delegate void playerHitDelegate(PlayerHealth p, int hitPoint);
    public event playerHitDelegate PlayerHit;

    private bool hasShield = false;
    private GameObject currentShield = null;

    public void HasShield(bool value, GameObject shield = null)
    {
        hasShield = value;
        if (value)
            currentShield = Instantiate(shield, shieldParent.position, Quaternion.identity, shieldParent);
        else
            Destroy(currentShield);
    }

    public void Hit(int hitPoint)
    {
        PlayerHit?.Invoke(this, hitPoint);

        if (!hasShield) {
            Debug.Log("Knock");
        }
    }

}
