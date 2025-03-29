using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextBoxUI : MonoBehaviour, IWindowPanel
{
    private Tween _tween;
    [SerializeField] private TextMeshProUGUI _tmp;
    
    private RectTransform _rectTrm => transform as RectTransform;

    private void Awake()
    {
        _rectTrm.localScale = Vector3.zero;
    }


    public void SetText(string text)
        =>_tmp.SetText(text);


    public void Open()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOScale(1, 0.2f).SetUpdate(true);
    }
    public void Close()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOScale(0, 0.2f).SetUpdate(true);
    }
}
