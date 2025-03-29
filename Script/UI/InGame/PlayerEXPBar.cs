using System;
using TMPro;
using UnityEngine;

public class PlayerEXPBar : MonoBehaviour
{
    [SerializeField] private RectTransform _expBar;
    [SerializeField] private TextMeshProUGUI _expText;

    private void Start()
    {
        PlayerManager.Instance.OnExpChangedEvent += HandleExpChangedEvent;
        HandleExpChangedEvent(0);
    }

    private void HandleExpChangedEvent(int value)
    {
        int max = PlayerManager.Instance.MaxExp;
        if (max == 0) max = 1;
        _expText.text = $"{value}/{max}";
        _expBar.sizeDelta = new Vector2(((float)value / max) * 600, 20);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnExpChangedEvent -= HandleExpChangedEvent;
    }
}
