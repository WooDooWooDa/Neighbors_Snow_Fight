using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnowGauge : NetworkBehaviour
{
    [SerializeField] private Slider gaugeSlider;

    [SerializeField] private TextMeshProUGUI ballAvailable;

    [SyncVar]
    private int snowGaugeValue = 0;

    private int maxSnowGauge = 3;

    private void Start()
    {
        gaugeSlider.maxValue = maxSnowGauge;
        gaugeSlider.value = snowGaugeValue;
        AddSnow(1);
    }

    private void Update()
    {
        gaugeSlider.value = snowGaugeValue;
        ballAvailable.text = (snowGaugeValue * 3).ToString();
    }

    public void Empty()
    {
        snowGaugeValue = 0;
    }

    public void Fill()
    {
        snowGaugeValue = maxSnowGauge;
    }

    [Server]
    public void AddSnow(int layer)
    {
        snowGaugeValue += layer;
        if (snowGaugeValue > maxSnowGauge) {
            snowGaugeValue = maxSnowGauge;
        }
    }

    [Server]
    public void UseSnow(int layer)
    {
        snowGaugeValue -= layer;
        if (snowGaugeValue < 0) {
            snowGaugeValue = 0;
        }
    }

    public bool Has(int amount)
    {
        return amount <= snowGaugeValue;
    }

    public bool CanReload()
    {
        return snowGaugeValue > 0;
    }

    public bool IsFull()
    {
        return snowGaugeValue == maxSnowGauge;
    }
}
