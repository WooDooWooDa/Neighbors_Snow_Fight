using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : MonoBehaviour
{
    [SerializeField] List<GameObject> snowBlockStates;
    [SerializeField] Transform parent;

    private int maxHitPoint = 4;
    private int hitPoint = 0;

    void Start()
    {
        SpawnBlock();
    }

    public void DamageBlock(int dmg)
    {
        Destroy(parent.GetChild(0).gameObject);
        hitPoint += dmg;
        if (hitPoint > maxHitPoint) {
            Destroy(gameObject);
            return;
        } else {
            SpawnBlock();
        }
    }

    private void SpawnBlock()
    {
        var block = Instantiate(snowBlockStates[hitPoint]);
        block.transform.SetParent(parent);
        block.transform.localPosition = Vector3.zero;
    }

}
