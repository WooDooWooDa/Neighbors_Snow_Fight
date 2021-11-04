using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private List<BaseItem> items = new List<BaseItem>();

    private List<BaseItem> removed = new List<BaseItem>();

    public void CollectItem(BaseItem item)
    {
        if (items.Count == 2) return;

        items.Add(item);
        item.gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (BaseItem item in items) {
            if (!item.HasBeenActivated())
                item.Activate(this);
            item.Update();
            if (item.GetTimeLeft() <= 0f || item.EffectIsDone())
                removed.Add(item);
        }
        if (removed.Count > 0)
            RemoveItems();
    }

    private void RemoveItems()
    {
        foreach (var item in removed) {
            items.Remove(item);
        }
        removed.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseItem>() != null) {
            CollectItem(other.GetComponent<BaseItem>());
        }
    }
}
