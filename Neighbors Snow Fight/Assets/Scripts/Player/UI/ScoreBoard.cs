using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private static readonly int YOU_INDEX = 0;
    private static readonly int ENEMY_INDEX = 1;

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


    void Update()
    {
        foreach (var score in playersScore) {
            if (score == GetComponentInParent<Score>()) {
                youScore.text = ScoreToString(score.GetScore());
            } else {
                enemyScore.text = ScoreToString(score.GetScore());
            }
        }
        scoreBall.UpdateSpeed(playersScore[YOU_INDEX].GetScore(), playersScore[ENEMY_INDEX].GetScore());
    }

    private string ScoreToString(float score)
    {
        return score.ToString("0");
    }
}
