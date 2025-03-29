using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerButtonUI : MonoBehaviour, IWindowPanel
{
    [field: SerializeField] public Button Button { get; private set; }
    [SerializeField] private TextMeshProUGUI _textCompo;
    private RectTransform _rectTrm => transform as RectTransform;
    private Tween _tween;

    public void SetText(string text)
        => _textCompo.SetText(text);

    public void Open()
    {
        if(_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOSizeDelta(new Vector2(125, 70), 0.2f).SetUpdate(true);
    }

    public void Close()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOSizeDelta(new Vector2(100, 70), 0.2f).SetUpdate(true);
    }

    public int GetLayer() => (int.Parse(_textCompo.text) - 1);
}
