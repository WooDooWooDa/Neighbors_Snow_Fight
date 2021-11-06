using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentPoints;

    void Start()
    {
        currentPoints = 0;
    }

    void Update()
    {
        //render
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
    }

    public void RemovePoints(int amount)
    {
        currentPoints -= amount;
        if (currentPoints <= 0)
            currentPoints = 0;
    }
}
