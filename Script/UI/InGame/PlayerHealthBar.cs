using System;
using TMPro;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private HealthCompo _healthCompo;
    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private RectTransform _changeHealthBar;
    [SerializeField] private TextMeshProUGUI _healthText;

    private float _targetGaugeAmount;
    private float _changeGaugeAmount;

    private float _lastChangeTime = 0;
        
    private void Start()
    {
        _healthCompo.OnHealthChangedEvent += HandleHealthChangedEvent;
        HandleHealthChangedEvent(_healthCompo.MaxHealth, _healthCompo.MaxHealth, true);
    }

    private void OnDisable()
    {
        _healthCompo.OnHealthChangedEvent -= HandleHealthChangedEvent;
    }

    private void Update()
    {
        if (_lastChangeTime + 0.5f < Time.time)
        {
            _changeGaugeAmount = _targetGaugeAmount;
            _lastChangeTime = Time.time;
        }

        _healthBar.sizeDelta = new Vector2(Mathf.Lerp(_healthBar.sizeDelta.x, _targetGaugeAmount * 600, Time.deltaTime * 8), 20);
        _changeHealthBar.sizeDelta = new Vector2(Mathf.Lerp(_changeHealthBar.sizeDelta.x, _changeGaugeAmount * 600, Time.deltaTime * 8), 20);
    }

    private void HandleHealthChangedEvent(int prev, int current, bool isChangingVisible)
    {
        if (isChangingVisible)
            _lastChangeTime = Time.time;
        _healthText.text = $"{current}/{_healthCompo.MaxHealth}";
        _targetGaugeAmount = (float)current / _healthCompo.MaxHealth;
    }
}
