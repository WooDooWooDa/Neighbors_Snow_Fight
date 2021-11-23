using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI youScore;
    [SerializeField] private TextMeshProUGUI enemyScore;

    [SerializeField] private ScoreBall scoreBall;

    private Score[] playersScore;

    void Start()
    {
        youScore.text = ScoreToString(0);
        enemyScore.text = ScoreToString(0);
        playersScore = GameObject.FindObjectsOfType<Score>();
    }

    public void UpdateScore(float you, float ennemy)
    {
        youScore.text = ScoreToString(you);
        enemyScore.text = ScoreToString(ennemy);
        scoreBall.UpdateSpeed(you, ennemy);
    }

    private string ScoreToString(float score)
    {
        return score.ToString("0");
    }
}
