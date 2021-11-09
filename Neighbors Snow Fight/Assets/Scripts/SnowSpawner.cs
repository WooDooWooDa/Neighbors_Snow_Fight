using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject snowLayerPrefab;
    [SerializeField] private Transform start;

    void Start()
    {
        InitializeSnow();
    }

    void Update()
    {
        
    }

    private void InitializeSnow()
    {
        for (int i = 0; i < 44; i++) {
            for (int j = 0; j < 25; j++) {
                GameObject layer = Instantiate(snowLayerPrefab, start);
                layer.transform.localPosition = new Vector3(i * -1.62f, 0, j * -1.62f);
            }

        }
    }
}
