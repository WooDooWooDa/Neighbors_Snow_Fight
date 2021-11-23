using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] players;
    private int maxPlayers = 2;

    private float maxGameTime = 120.0f; //2 minutes
    private float currentGameTime;

    void Start()
    {
        if (!isServer) return;

        GetPlayers();
        currentGameTime = maxGameTime;
        //StartGame();
    }

    
    void Update()
    {
        if (!isServer) return;
        if (players.Length != maxPlayers)
            GetPlayers();

        UpdateTime(); //do in game loop
        UpdateScore();
    }

    [TargetRpc]
    private void RpcUpdateTime(NetworkConnection conn, float time)
    {
        conn.identity.gameObject.GetComponentInChildren<PlayerTime>().SetTime(time);
    }

    [TargetRpc]
    private void RpcUpdateScore(NetworkConnection conn, float scorePlayer, float scoreEnnemy)
    {
        conn.identity.gameObject.GetComponentInChildren<ScoreBoard>().UpdateScore(scorePlayer, scoreEnnemy);
    }

    [Server]
    private void UpdateTime()
    {
        currentGameTime -= Time.deltaTime;
        foreach (var player in players)
        {
            RpcUpdateTime(player.GetComponent<NetworkIdentity>().connectionToClient, currentGameTime);
        }
    }

    [Server]
    private void UpdateScore()
    {
        foreach (var player in players)
        {
            var tempList = players.ToList();
            tempList.Remove(player);
            var playerScore = player.GetComponent<Score>().GetScore();
            var ennemyScore = tempList.First().GetComponent<Score>().GetScore();
            RpcUpdateScore(player.GetComponent<NetworkIdentity>().connectionToClient, playerScore, ennemyScore);
        }
    }

    [Server]
    private void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
