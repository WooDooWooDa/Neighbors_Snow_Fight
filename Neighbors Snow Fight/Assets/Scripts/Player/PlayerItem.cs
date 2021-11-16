using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : NetworkBehaviour
{
    [SerializeField] private List<BaseItem> items = new List<BaseItem>();
    [SerializeField] private ItemsDisplay itemsDisplay;

    public delegate void playerCollectItemDelegate(PlayerItem p, BaseItem item);
    public delegate void playerItemRemoveDelegate(PlayerItem p, BaseItem item);
    public event playerCollectItemDelegate PlayerCollectItem;
    public event playerItemRemoveDelegate PlayerItemRemove;

    private List<BaseItem> removed = new List<BaseItem>();

    public List<BaseItem> GetItems()
    {
        return items;
    }

    private void CollectItem(BaseItem item)
    {
        if (items.Count == 2) return;

        items.Add(item);
    }

    private void Update()
    {
        if (!isServerOnly) return;

        foreach (BaseItem item in items) {
            if (!item.HasBeenActivated())
                item.Activate(this);
            item.Update();
            RpcUpdateUI(GetComponent<NetworkIdentity>().connectionToServer, item);
            if (item.GetTimeLeft() <= 0f || item.EffectIsDone()) {
                RpcRemoveItem(GetComponent<NetworkIdentity>().connectionToServer, item);
                removed.Add(item);
            }
        }
        if (removed.Count > 0)
            RemoveItems();
    }

    [Server]
    private void RemoveItems()
    {
        Debug.LogWarning("Removing items");
        foreach (BaseItem item in removed) {
            items.Remove(item);
            NetworkServer.Destroy(item.gameObject);
        }
        removed.Clear();
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseItem>() != null) {
            CollectItem(other.GetComponent<BaseItem>());
            RpcCollectItem(GetComponent<NetworkIdentity>().connectionToServer, other.GetComponent<BaseItem>());
            other.gameObject.SetActive(false);
            RpcDesactiveItem(other.gameObject);
        }
    }

    [ClientRpc]
    private void RpcDesactiveItem(GameObject item)
    {
        item.gameObject.SetActive(false);
    }

    [TargetRpc]
    private void RpcUpdateUI(NetworkConnection conn, BaseItem item)
    {
        itemsDisplay.UpdateSlot(item);
    }

    [TargetRpc]
    private void RpcRemoveItem(NetworkConnection conn, BaseItem item)
    {
        Debug.Log("Removing item : " + item.name);
        PlayerItemRemove?.Invoke(this, item);
    }

    [TargetRpc]
    private void RpcCollectItem(NetworkConnection conn, BaseItem item)
    {
        Debug.Log("Collect item : " + item.name);
        PlayerCollectItem?.Invoke(this, item);
    }
}

