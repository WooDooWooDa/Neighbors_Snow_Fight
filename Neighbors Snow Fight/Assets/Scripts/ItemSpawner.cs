using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> items; //implement rarity of item in spawn!
    [SerializeField] private Collider spawnPlane;

    private float itemBaseSpawnRate = 10f;
    private float currentSpawnRate;
    private float itemTimer = 0f;

    private bool isActive = false;
    
    public void Activate(bool active)
    {
        isActive = active;
    }

    public void SetSpawnRate(float value)
    {
        currentSpawnRate = itemBaseSpawnRate * value;
    }

    private void Start()
    {
        currentSpawnRate = itemBaseSpawnRate;
    }

    void Update()
    {
        if (!isServer) return;
        if (!isActive) return;

        itemTimer += Time.deltaTime;
        if (itemTimer >= currentSpawnRate)
        {
            Spawn();
            itemTimer = 0f;
        }
    }

    private void Spawn()
    {
        NetworkServer.Spawn(Object.Instantiate(items[Random.Range(0, items.Count)], RandomPointInBounds(spawnPlane.bounds), Quaternion.identity));
    }

    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y) - 1,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
