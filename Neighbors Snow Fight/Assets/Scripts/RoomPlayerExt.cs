using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerExt : NetworkRoomPlayer
{
    public override void OnClientEnterRoom()
    {
        if (!isLocalPlayer) return;
        Debug.Log($"Player entered lobby : Player #{index}");
    }

    public override void OnClientExitRoom()
    {
        
    }

    public override void OnStartLocalPlayer()
    {
        Button readyButton = GameObject.FindGameObjectWithTag("ReadyBtn").GetComponent<Button>();
        readyButton.onClick.AddListener(ReadyUp);
    }

    private void ReadyUp()
    {
        if (!isLocalPlayer) return;

        base.CmdChangeReadyState(!readyToBegin);
        TextMeshProUGUI readyText = GameObject.FindGameObjectWithTag("ReadyBtn").GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log($"ready : {readyToBegin}");
        readyText.text = !readyToBegin ? "Cancel" : "Ready up!";
    }
}
