using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : BaseItem
{
    [SerializeField] private GameObject shield;

    public delegate void playerHitDelegate(PlayerHealth p, int point);

    private int maxHitPoint = 4;
    private int hitPoint;

    public override void ApplyEffect()
    {
        hitPoint = maxHitPoint;
        playerItem.GetComponent<PlayerHealth>().HasShield(true, shield);
        playerItem.GetComponent<PlayerHealth>().PlayerHit += GotHit;
    }

    public override void EndEffect()
    {
        playerItem.GetComponent<PlayerHealth>().HasShield(false);
    }

    public override void UpdateItem()
    {
        if (hitPoint <= 0)
            effectIsDone = true;
    }

    public void GotHit(PlayerHealth health, int point)
    {
        hitPoint -= point;
    }
}
