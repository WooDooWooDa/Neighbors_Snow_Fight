using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] private float baseTime;

    private PlayerItem playerItem;

    private float timeLeft;
    private bool hasBeenActivated;

    void Start()
    {
        timeLeft = baseTime;
        Destroy(gameObject, 15);
    }

    private void Update()
    {
        if (hasBeenActivated) {
            LowerTime();

            if (timeLeft == 0) {
                EndEffect(playerItem);
                Destroy(gameObject);
            }
        }
    }

    public abstract void ApplyEffect(PlayerItem playerItem);

    public abstract void EndEffect(PlayerItem playerItem);

    public string GetTimeLeft()
    {
        return timeLeft.ToString("0");
    }

    public void Activate(PlayerItem playerItem)
    {
        this.playerItem = playerItem;
        hasBeenActivated = true;
        ApplyEffect(playerItem);
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
}
