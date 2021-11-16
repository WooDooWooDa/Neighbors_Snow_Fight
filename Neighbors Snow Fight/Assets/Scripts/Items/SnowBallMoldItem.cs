using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallMoldItem : BaseItem
{
    public delegate void playerReloadDelegate(PlayerShoot p);

    private int uses = 0;
    private int maxUse = 3;

    public override void ApplyEffect()
    {
        var shoot = playerItem.GetComponent<PlayerShoot>();
        shoot.UseMold(true);
        shoot.PlayerReload += UpdateUses;
    }

    public override void EndEffect()
    {
        playerItem.GetComponent<PlayerShoot>().UseMold(false);
    }

    public override void UpdateItem()
    {
        if (uses == maxUse)
            effectIsDone = true;
    }

    private void UpdateUses(PlayerShoot player)
    {
        uses++;
    }
}
