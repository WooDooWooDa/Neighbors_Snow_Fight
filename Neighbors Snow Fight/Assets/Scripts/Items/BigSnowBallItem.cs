using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowBallItem : BaseItem
{
    [SerializeField] private GameObject bigBallPrefab;

    public override void UpdateItem()
    {
        if (!playerItem.GetComponent<PlayerShoot>().HasBall()) {
            effectIsDone = true;
        }
    }

    public override void ApplyEffect()
    {
        (playerItem.GetComponent<PlayerShoot>()).ReplaceBall(bigBallPrefab);
    }

    public override void EndEffect()
    {
        (playerItem.GetComponent<PlayerShoot>()).ReplaceBall(null);
    }
}
