using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsParent;
    private Dictionary<BaseItem, ItemSlot> slots;

    public void UpdateSlot(BaseItem item)
    {
        if (slots.TryGetValue(item, out var slot))
            slot.UpdateTime(item.GetTimeLeft());
    }

    private void Start()
    {
        slots = new Dictionary<BaseItem, ItemSlot>();
        var playerItem = GetComponentInParent<PlayerItem>();
        playerItem.PlayerCollectItem += AddItem;
        playerItem.PlayerItemRemove += RemoveItem;
    }

    private void Update()
    {
        var index = 0;
        foreach (KeyValuePair<BaseItem, ItemSlot> slot in slots) {
            slot.Value.gameObject.transform.localPosition = new Vector3(0, index == 1 ? 100 : 0, 0);
            index++;
        }
    }

    private void AddItem(PlayerItem playerItem, BaseItem item)
    {
        ItemSlot slot = Instantiate(slotPrefab, slotsParent).GetComponent<ItemSlot>();
        slot.Initiate(item);
        slots.Add(item, slot);
    }

    private void RemoveItem(PlayerItem playerItem, BaseItem item)
    {
        if (slots.TryGetValue(item, out var slot)){
            Destroy(slot.gameObject);
            slots.Remove(item);
        }
    }

}
