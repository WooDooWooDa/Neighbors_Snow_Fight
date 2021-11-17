using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform ball;

    private Vector3 baseBallScale = new Vector3(1, 1, 1);
    private Vector3 currentSize;

    void Start()
    {
        currentSize = baseBallScale;
        ball.localScale = currentSize;
    }

    public void ShowHand(bool show)
    {
        hand.SetActive(show);
    }

    public void ChangeBallSize(float size)
    {
        currentSize = baseBallScale * size;
    }
}
