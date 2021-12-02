using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBall : MonoBehaviour
{
    [SerializeField] private Transform ball;

    private float speed;

    void Start()
    {
        speed = 0;
    }

    void Update()
    {
        var rotation = new Vector3(0, 0, speed / 2);
        ball.Rotate(rotation);
    }

    public void UpdateSpeed(float youScore, float enemyScore)
    {
        var tempSpeed = 1f;
        if (youScore == enemyScore) {
            tempSpeed = 0;
        } else if (youScore > enemyScore) {
            if (enemyScore != 0) {
                tempSpeed *= 1f * (youScore / enemyScore);
            } else {
                tempSpeed *= 1f * youScore;
            }
        } else {
            if (youScore != 0) {
                tempSpeed *= -1f * (enemyScore / youScore);
            } else {
                tempSpeed *= -1f * enemyScore;
            }
        }
        speed = tempSpeed;
    }
}
