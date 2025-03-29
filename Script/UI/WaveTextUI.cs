
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class WaveTextUI : MonoBehaviour
{
    private TextMeshProUGUI _waveText;
    private void Awake()
    {
        WaveManager.Instance.OnWaveStart += HandleWaveStart;
        _waveText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HandleWaveStart();
        }
    }

    private void HandleWaveStart()
    {
        _waveText.text = $"Wave : {WaveManager.Instance.waveCount - 1}";
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(35f, 0.5f));
        seq.Insert(1.5f, transform.DOMoveY(100f, 0.5f));
    }
}
