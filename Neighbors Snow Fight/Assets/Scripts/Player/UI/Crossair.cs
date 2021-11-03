using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crossair : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crossair;

    private void Start()
    {
        SetColor(new Color(PlayerPrefs.GetFloat("crossairColorR", 255), PlayerPrefs.GetFloat("crossairColorG", 255), PlayerPrefs.GetFloat("crossairColorB", 255)));
    }

    public void SetColor(Color color)
    {
        crossair.color = color;
    }
}
