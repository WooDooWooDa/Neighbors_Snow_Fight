using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitlessGaugeItem : BaseItem
{
    public override void ApplyEffect()
    {
        playerItem.GetComponent<SnowGauge>().Fill();
    }

    public override void EndEffect()
    {
        
    }

    public override void UpdateItem()
    {
        var gauge = playerItem.GetComponent<SnowGauge>();
        if (!gauge.IsFull())
            gauge.Fill();
    }
}
