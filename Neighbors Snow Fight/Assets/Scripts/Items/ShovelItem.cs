using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelItem : BaseItem
{
    public delegate void playerCollectSnowDelegate(CollectSnow p);

    private int snowLayerPicked = 0;
    private int maxLayer = 6;

    public override void ApplyEffect()
    {
        playerItem.GetComponent<CollectSnow>().SetDelay(0.5f);
        playerItem.GetComponent<CollectSnow>().PlayerCollectSnow += UpdateSnowLayerPickUp;
    }

    public override void EndEffect()
    {
        playerItem.GetComponent<CollectSnow>().SetDelay(1f);
    }

    public override void UpdateItem()
    {
        if (snowLayerPicked == maxLayer) {
            effectIsDone = true;
        }
    }

    private void UpdateSnowLayerPickUp(CollectSnow snow)
    {
        if (snow.GetComponent<PlayerItem>() != playerItem) return;
        snowLayerPicked++;
    }
}
