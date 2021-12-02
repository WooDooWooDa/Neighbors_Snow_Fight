using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private int maxPlayers = 2;
    private int nbConnPlayers;

    [Header("Music")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip waitingMusic;
    [SerializeField] AudioClip roundMusic;

    [Header("Sounds")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] List<AudioClip> readySetGoSounds;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip tieSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;


    [Header("Game")]
    [SerializeField] private float maxGameTime = 120.0f; //2 minutes
    [SerializeField] private float snowStormTime = 60.0f; //2 minutes
    private SnowStorm snowStorm;
    private float currentGameTime;
    private bool roundIsPlaying = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameObject.Find("OfflineCamera").SetActive(false);
    }

    public override void OnStartServer()
    {
        Initialize(NetworkManager.singleton.numPlayers);
        snowStorm = GetComponent<SnowStorm>();
        StartGame();
    }

    [Server]
    public void Initialize(int nbPlayers)
    {
        nbConnPlayers = nbPlayers;
        if (!isServerOnly) return;

        GetPlayers();
        currentGameTime = maxGameTime;
    }

    [Server]
    private void StartGame()
    {
        if (isServerOnly)
            StartCoroutine(GameLoop());
    }

    [Server]
    private IEnumerator GameLoop()
    {
        while (players.Length < minPlayers) {  //waiting for players
            GetPlayers();
            RpcMessage("Waiting for players...", 1000);
            RpcPlayWaitingMusic();
            yield return null;
        }
        RpcMessage("", 1);

        yield return StartCoroutine(RoundStart());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnd());
    }

    [Server]
    private IEnumerator RoundStart()
    {
        yield return new WaitForSeconds(2);
        RpcPlayStartRoundSound(0, "READY!");
        yield return new WaitForSeconds(1);
        RpcPlayStartRoundSound(1, "SET!");
        yield return new WaitForSeconds(1);
        RpcPlayStartRoundSound(2, "GOOO!!");
    }

    [Server]
    private IEnumerator RoundPlaying()
    {
        RpcPlayCombatMusic();
        TogglePlayerActions(true);
        roundIsPlaying = true;
        GetComponent<ItemSpawner>().Activate(true);
        while (currentGameTime > 0f) {
            yield return null;
        }
    }

    [Server]
    private IEnumerator RoundEnd()
    {
        TogglePlayerActions(false);
        GetComponent<ItemSpawner>().Activate(false); 
        roundIsPlaying = false;
        RpcGameOver();
        var winner = CalculateWinner();
        yield return new WaitForSeconds(4);
        foreach (var player in players) {
            RpcGameOverWL(player.GetComponent<NetworkIdentity>().connectionToClient, player.GetComponent<Score>() == winner);
        }
    }

    private void Update()
    {
        if (!isServerOnly) return;

        if (!roundIsPlaying) return;

        if (players.Length != maxPlayers)
            GetPlayers();

        UpdateTime();
        UpdateScore();

        if (currentGameTime <= snowStormTime && !snowStorm.IsActive())
            snowStorm.StartSnowStorm();
    }

    [Server]
    private Score CalculateWinner()
    {
        Score tempWinner = null;
        foreach (var player in players) {
            Score playerScore = player.GetComponent<Score>();
            if (tempWinner == null || playerScore.GetScore() > tempWinner.GetScore()) {
                tempWinner = playerScore;
            }
        }
        return tempWinner;
    }

    [Server]
    private void TogglePlayerActions(bool toggle)
    {
        foreach (var player in players) {
            player.GetComponent<PlayerMouvement>().canMove = toggle;
            player.GetComponent<PlayerShoot>().canShoot = toggle;
            player.GetComponent<CollectSnow>().canCollect = toggle;
        }
    }

    [Server]
    private void UpdateTime()
    {
        currentGameTime -= Time.deltaTime;
        foreach (var player in players) {
            RpcUpdateTime(player.GetComponent<NetworkIdentity>().connectionToClient, currentGameTime);
        }
    }

    [Server]
    private void UpdateScore()
    {
        foreach (var player in players) {
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

    [ClientRpc]
    private void RpcMessage(string message, int duration)
    {
        MessageAnnoncer.Duration = duration;
        MessageAnnoncer.Message = message;
    }

    [ClientRpc]
    private void RpcPlayStartRoundSound(int index, string countdown)
    {
        MessageAnnoncer.Message = countdown;
        soundSource.clip = readySetGoSounds[index];
        soundSource.Play();
    }

    [ClientRpc]
    private void RpcPlayCombatMusic()
    {
        musicSource.clip = roundMusic;
        musicSource.Play();
    }

    [ClientRpc]
    private void RpcPlayWaitingMusic()
    {
        if (musicSource.isPlaying && musicSource.clip == waitingMusic) return;
        musicSource.clip = waitingMusic;
        musicSource.Play();
    }

    [ClientRpc]
    private void RpcGameOver()
    {
        soundSource.clip = gameOverSound;
        soundSource.Play();
        MessageAnnoncer.FontSize = 50;
        MessageAnnoncer.Duration = 100;
        MessageAnnoncer.Message = "Time over!";
    }

    [TargetRpc]
    private void RpcGameOverWL(NetworkConnection conn, bool winner)
    {
        if (winner) {
            MessageAnnoncer.Message = "You are the winner!!";
            soundSource.clip = winSound;
        } else {
            MessageAnnoncer.Message = "You have lost the match...";
            soundSource.clip = loseSound;
        }
        soundSource.Play();

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
}
