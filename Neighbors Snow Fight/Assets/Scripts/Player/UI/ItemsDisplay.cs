using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ItemsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsParent;
    private Dictionary<ItemSlot, BaseItem> slots;

    private void Start()
    {
        slots = new Dictionary<ItemSlot, BaseItem>();
        var playerItem = GetComponentInParent<PlayerItem>();
        playerItem.PlayerCollectItem += AddItem;
        playerItem.PlayerItemRemove += RemoveItem;
    }

    private void Update()
    {
        var index = 0;
        foreach (KeyValuePair<ItemSlot, BaseItem> slot in slots)
        {
            slot.Key.gameObject.transform.localPosition = new Vector3(0, index == 1 ? 100 : 0, 0);
            index++;
        }
    }

    private void AddItem(PlayerItem playerItem, BaseItem item)
    {
        ItemSlot slot = Instantiate(slotPrefab, slotsParent).GetComponent<ItemSlot>();
        slot.Initiate(item);
        slots.Add(slot, item);
    }

    private void RemoveItem(PlayerItem playerItem, BaseItem item)
    {
        foreach (KeyValuePair<ItemSlot, BaseItem> slot in slots) {
            if (slot.Value == item) {
                Debug.Log("Destroying item slot");
                Destroy(slot.Key.gameObject);
                slots.Remove(slot.Key);
            }
        }
    }
    
}
