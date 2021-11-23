using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLeft;

    public void SetTime(float time)
    {
        timeLeft.text = FormatTime(time);
    }

    private string FormatTime(float time)
    {
        TimeSpan newTime = TimeSpan.FromSeconds(time);
        return newTime.ToString(@"mm\:ss");
    }
}
