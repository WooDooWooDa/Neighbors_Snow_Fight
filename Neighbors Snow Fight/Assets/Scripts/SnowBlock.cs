using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : MonoBehaviour
{
    [SerializeField] List<GameObject> snowBlockStates;
    [SerializeField] Transform parent;

    public delegate void playerBlockDestroyedDelegate(SnowBlock p);
    public event playerBlockDestroyedDelegate BlockDestroyed;

    private PlaceBlock player;

    private int maxHitPoint = 4;
    private int hitPoint = 0;

    void Start()
    {
        SpawnBlocks();
    }

    private void Update()
    {
        if (hitPoint >= maxHitPoint) {
            Destroy(parent.gameObject);
            BlockDestroyed?.Invoke(this);
        }
    }

    public bool BelongsTo(PlayerShoot launcher)
    {
        return player == launcher.GetComponent<PlaceBlock>();
    }

    public void SetPlayer(PlaceBlock player)
    {
        this.player = player;
    }

    public bool DamageBlock(int dmg)
    {
        Destroy(parent.GetChild(0).gameObject);
        hitPoint += dmg;
        if (hitPoint >= maxHitPoint) {
            return true;
        } else {
            SpawnBlocks();
            return false;
        }
    }

    private void SpawnBlocks()
    {
        var block = Instantiate(snowBlockStates[hitPoint], parent);
        block.transform.localRotation = Quaternion.identity;
        if (hitPoint == 0) {
            block.transform.localPosition = Vector3.zero;
        } else {
            block.transform.localPosition = new Vector3(0, -0.8f, 0.1f);
            block.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

}
