using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGolemItem : BaseItem
{
    [SerializeField] private GameObject snowGolem;

    private GameObject spawnedGolem = null;

    private int maxShot = 9;
    private int shooted = 0;

    private int cooldown = 1;
    private float elapsed = 1;

    public override void ApplyEffect()
    {
        spawnedGolem = Instantiate(snowGolem, this.transform.position + (Vector3.up * 5), snowGolem.transform.rotation);
        spawnedGolem.GetComponent<SnowGolem>().SetPlayer(playerItem);
    }

    public override void EndEffect()
    {
        Destroy(spawnedGolem);
    }

    public override void UpdateItem()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= cooldown) {
            elapsed = 0;
            spawnedGolem.GetComponent<SnowGolem>().LaunchBall();
            shooted++;
        }
        if (shooted == maxShot) {
            effectIsDone = true;
        }
    }
}
