using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLayer : MonoBehaviour
{
    [SerializeField] private List<GameObject> layers;
    [SerializeField] private Transform parent;

    void Start()
    {
        var index = Random.Range(0, 2);
        var layer = Instantiate(layers[index]);
        layer.transform.SetParent(parent);
        layer.transform.localScale = Vector3.one / 2;
        layer.transform.localPosition = new Vector3(0, index == 0 ? 0.35f : 0.33f, 0);
        GetComponent<BoxCollider>().size = new Vector3(1.6f, 0.05f * (index + 1), 1.6f);
    }

    public void Take()
    {
        Destroy(gameObject);
    }
}
