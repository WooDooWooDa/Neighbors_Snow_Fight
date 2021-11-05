using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBallItem : BaseItem
{
    public delegate void playerReloadDelegate(PlayerShoot p);

    private int maxBalls = 4;
    private int uses = 0;
    private int maxUses = 4;

    public override void ApplyEffect()
    {
        var shoot = playerItem.GetComponent<PlayerShoot>();
        shoot.ChangeMaxBalls(false, maxBalls);
        shoot.PlayerReload += UpdateUses;
    }

    public override void EndEffect()
    {
        playerItem.GetComponent<PlayerShoot>().ChangeMaxBalls(true);
    }

    public override void UpdateItem()
    {
        if (uses == maxUses)
            effectIsDone = true;
    }

    private void UpdateUses(PlayerShoot player)
    {
        if (player.GetComponent<PlayerItem>() != playerItem) return;
        uses++;
    }
}
