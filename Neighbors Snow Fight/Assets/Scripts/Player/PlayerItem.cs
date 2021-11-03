using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private List<BaseItem> items = new List<BaseItem>();

    public void CollectItem(BaseItem item)
    {
        if (items.Count == 2) return;

        items.Add(item);
        item.gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (BaseItem item in items)
        {
            if (!item.HasBeenActivated())
                item.Activate(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseItem>() != null) {
            CollectItem(other.GetComponent<BaseItem>());
        }
    }
}
