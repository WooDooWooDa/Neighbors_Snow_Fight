using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlock : MonoBehaviour
{
    [SerializeField] private LayerMask bound;
    [SerializeField] private GameObject snowBlock;
    [SerializeField] private GameObject snowBlockFrame;

    public delegate void playerBlockDestroyedDelegate(SnowBlock p);

    private GameObject spawnedFrame;

    private float dist = 10f;
    private int snowCost = 3;

    private int maxBlocks = 8;
    private int placedBlocks = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleFrame();
        }
        if (spawnedFrame != null) {
            MoveFrame();
            if (Input.GetKeyDown(KeyCode.Mouse1) && GetComponent<SnowGauge>().Has(snowCost) && placedBlocks < maxBlocks) {
                Place();
            }
        }
    }

    private void Place()
    {
        GetComponent<SnowGauge>().UseSnow(snowCost);
        Transform frame = spawnedFrame.transform;
        SnowBlock block = (Instantiate(snowBlock, frame.position + (Vector3.up * 1), frame.rotation)).GetComponent<SnowBlock>();
        RandomRotate(block.transform);
        block.SetPlayer(this);
        block.BlockDestroyed += BlockDestroyed;
        ToggleFrame();
        placedBlocks++;
    }

    private void BlockDestroyed(SnowBlock block)
    {
        placedBlocks--;
    }

    private void MoveFrame()
    {
        var direction = GetComponentInChildren<MouseLook>().GetDirection();
        var position = GetComponentInChildren<MouseLook>().GetPosition();
        if(Physics.Raycast(position, direction * Vector3.forward, out RaycastHit hit, dist)) {
            Debug.LogWarning(hit.collider.gameObject.layer);
            if (!((bound.value & (1 << hit.collider.gameObject.layer)) > 0)) {
                spawnedFrame.SetActive(true);
                spawnedFrame.transform.position = hit.point;
            } else
                spawnedFrame.SetActive(false);
        } else { 
            spawnedFrame.SetActive(false);
        }
    }

    private void RandomRotate(Transform block)
    {
        block.localRotation = Quaternion.Euler(Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90);
    }

    private void ToggleFrame()
    {
        if (spawnedFrame != null)
            Destroy(spawnedFrame);
        else {
            spawnedFrame = Instantiate(snowBlockFrame, this.transform);
            spawnedFrame.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }
}
