using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : NetworkBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float baseTime;
    [SerializeField] private Rarity rarity;

    protected PlayerItem playerItem;
    protected bool effectIsDone = false;
    protected bool isSuper = false;

    private float timeLeft;
    private bool hasBeenActivated;

    void Start()
    {
        timeLeft = baseTime;
        StartCoroutine(DestroyAfter15());
    }

    public GameObject GetModel()
    {
        return model;
    }

    public void Update()
    {
        if (hasBeenActivated) {
            UpdateItem();
            LowerTime();
            if (timeLeft == 0 || effectIsDone) {
                EndEffect();
                effectIsDone = true;
                Destroy(gameObject);
            }
        }
    }

    public void Activate(PlayerItem playerItem)
    {
        this.playerItem = playerItem;
        hasBeenActivated = true;
        ApplyEffect();
    }

    public abstract void UpdateItem();

    public abstract void ApplyEffect();

    public abstract void EndEffect();

    public float GetTimeLeft()
    {
        return timeLeft;
    }

    public bool EffectIsDone()
    {
        return effectIsDone;
    }

    public bool HasBeenActivated()
    {
        return hasBeenActivated;
    }

    private void LowerTime()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            timeLeft = 0;
        }
    }

    private IEnumerator DestroyAfter15()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        StopCoroutine(DestroyAfter15());
    }

    private enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Unique
    }
}
