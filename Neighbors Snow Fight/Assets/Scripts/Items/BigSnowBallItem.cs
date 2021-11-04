using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowBallItem : BaseItem
{
    [SerializeField] private Rigidbody bigBallPrefab;

    public override void UpdateItem(PlayerItem playerItem)
    {
        if (!playerItem.GetComponent<PlayerShoot>().HasBall()) {
            Debug.LogWarning("No balls left");
            effectIsDone = true;
        }
    }

    public override void ApplyEffect(PlayerItem playerItem)
    {
        (playerItem.GetComponent<PlayerShoot>()).ReplaceBall(bigBallPrefab);
    }

    public override void EndEffect(PlayerItem playerItem)
    {
        (playerItem.GetComponent<PlayerShoot>()).ReplaceBall(null);
    }
}
