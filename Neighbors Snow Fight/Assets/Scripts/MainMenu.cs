using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkRoomManager networkManager;
    [SerializeField] private OptionsMenu options;

    private void Start()
    {
        options.Initialize();
    }

    public void JoinServer()
    {
        if (networkManager == null) {
            networkManager = FindObjectOfType<NetworkRoomManager>();
        }
        try {
            networkManager.networkAddress = "159.203.6.221";
            networkManager.StartClient();
        } catch (Exception e) {
            Debug.Log(e);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
