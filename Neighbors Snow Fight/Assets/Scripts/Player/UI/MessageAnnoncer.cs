using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageAnnoncer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI annoncer;
    public enum Type {
        Alert,
        Message,
        Warning
    }

    public static string Message { get; set; }

    public static float Duration { get; set; }

    public static int FontSize { get; set; }

    public static Type MessageType { get; set; }

    private float elapsed;
    private float defaultDuration = 3;
    private int defaultFontSize = 25;
    
    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (Message != null) {
            annoncer.fontSize = FontSize;
            SetColor();
            annoncer.text = Message;
            if (Duration < elapsed) {
                Reset();
            }
            elapsed += Time.deltaTime;
        }
    }

    private void SetColor()
    {
        switch (MessageType) {
            case Type.Alert: annoncer.color = Color.red; break;
            case Type.Warning: annoncer.color = Color.yellow; break;
            case Type.Message: annoncer.color = Color.white; break;
        }
    }

    private void Reset()
    {
        annoncer.text = "";
        elapsed = 0;
        Duration = defaultDuration;
        FontSize = defaultFontSize;
        MessageType = Type.Message;
        Message = null;
    }
}
