using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] private float baseTime;

    protected PlayerItem playerItem;

    private float timeLeft;
    private bool hasBeenActivated;

    protected bool effectIsDone = false;

    void Start()
    {
        timeLeft = baseTime;
        StartCoroutine(DestroyAfter15());
    }

    public void Update()
    {
        if (hasBeenActivated) {
            UpdateItem(playerItem);
            LowerTime();
            if (timeLeft == 0 || effectIsDone) {
                EndEffect(playerItem);
                Destroy(gameObject);
            }
        }
    }

    public void Activate(PlayerItem playerItem)
    {
        this.playerItem = playerItem;
        hasBeenActivated = true;
        ApplyEffect(playerItem);
    }

    public abstract void UpdateItem(PlayerItem playerItem);

    public abstract void ApplyEffect(PlayerItem playerItem);

    public abstract void EndEffect(PlayerItem playerItem);

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
}
