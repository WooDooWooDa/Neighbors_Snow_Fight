using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGoggleItem : BaseItem
{


    public override void ApplyEffect()
    {
        RenderSettings.fog = false;
    }

    public override void EndEffect()
    {
        RenderSettings.fog = true;
    }

    public override void UpdateItem()
    {
        
    }
}
