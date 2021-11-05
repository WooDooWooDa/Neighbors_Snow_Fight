using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : BaseItem
{
    public override void ApplyEffect()
    {
        playerItem.GetComponent<PlayerMouvement>().SetSpeed(1.5f);
    }

    public override void EndEffect()
    {
        playerItem.GetComponent<PlayerMouvement>().SetSpeed(1f);
    }

    public override void UpdateItem()
    {
        
    }
}
