using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLayer : MonoBehaviour
{
    [SerializeField] private GameObject layerPrefab;
    [SerializeField] private Transform filling;

    private int maxLayers = 6;
    private int currentLayers;

    void Start()
    {
        currentLayers = Random.Range(1, maxLayers + 1);
        var layer = Instantiate(layerPrefab, filling);
        layer.transform.localScale = Vector3.one / 2;
        layer.transform.localPosition = new Vector3(0, 0.35f, 0);
    }

    private void Update()
    {
        filling.transform.localScale = new Vector3(1, currentLayers, 1);
        filling.transform.localPosition = new Vector3(0, ((currentLayers - 1) * 0.03f), 0.05f);
    }

    public void AddSnow(int amount)
    {
        currentLayers += currentLayers;
        if (currentLayers > maxLayers)
            currentLayers = maxLayers;
    }

    public void Take()
    {
        currentLayers--;
        if (currentLayers == 0)
            Destroy(gameObject);
    }
}
