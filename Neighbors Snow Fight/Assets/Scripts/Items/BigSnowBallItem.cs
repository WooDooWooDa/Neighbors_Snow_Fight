using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowBallItem : BaseItem
{
    [SerializeField] private Rigidbody bigBallPrefab;

    public override void ApplyEffect(PlayerItem playerItem)
    {
        (playerItem.GetComponent<PlayerShoot>()).ReplaceBall(bigBallPrefab);
    }

    public override void EndEffect(PlayerItem playerItem)
    {
        
    }
}
