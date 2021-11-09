using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Transform modelParent;
    [SerializeField] private TextMeshProUGUI timeLeft;

    private BaseItem item;

    public void Initiate(BaseItem item)
    {
        this.item = item;
        SpawnModel();
    }

    private void SpawnModel()
    {
        var model = Instantiate(item.GetModel(), modelParent);
        model.transform.localScale = new Vector3(25, 25, 1);
    }

    void Update()
    {
        timeLeft.text = item.GetTimeLeft().ToString("0") + " sec";
        if (item.GetTimeLeft() <= 0 || item == null) {
            
        }
    }
}
