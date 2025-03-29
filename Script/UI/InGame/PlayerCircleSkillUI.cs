using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCircleSkillUI : MonoBehaviour
{
    [SerializeField] private Image _gaugeMaskImage;
    [SerializeField] private Image _gaugeImage;

    [SerializeField, ColorUsage(true, true)] private Color _activeColor;
    [SerializeField, ColorUsage(true, true)] private Color _activeBlinkColor;
    [ColorUsage(true, true)] private Color _defaultColor;

    private readonly int _colorHash = Shader.PropertyToID("_Emission");

    private float _targetAmount = 0;
    private Material _material;

    private void Start()
    {
        PlayerManager.Instance.OnChangedPlayerCircleSkillGaugeEvent += HandleChangedPlayerCircleSkillGaugeEvent;
        PlayerManager.Instance.OnPlayerCircleSkillActiveEvent += HandlePlayerCircleSkillActiveEvent;
        PlayerManager.Instance.OnCircleSkillEvent += HandleCircleSkillEvent;
        HandleChangedPlayerCircleSkillGaugeEvent(PlayerManager.Instance.CurrentPlayerCircleSkillGauge);

        _material = _gaugeImage.materialForRendering;
        _defaultColor = _material.GetColor(_colorHash);
    }

    private void HandleCircleSkillEvent(bool value)
    {
        if (value)
            Blink(_activeBlinkColor, 0.2f);
        else
        {
            Blink(_defaultColor, 0.2f);
        }
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnChangedPlayerCircleSkillGaugeEvent -= HandleChangedPlayerCircleSkillGaugeEvent;
            PlayerManager.Instance.OnPlayerCircleSkillActiveEvent -= HandlePlayerCircleSkillActiveEvent;
            PlayerManager.Instance.OnCircleSkillEvent -= HandleCircleSkillEvent;
        }
    }

    private void Update()
    {
        if (_gaugeMaskImage.fillAmount < _targetAmount)
            _gaugeMaskImage.fillAmount = Mathf.Lerp(_gaugeMaskImage.fillAmount, _targetAmount, Time.deltaTime * 12);
        else
            _gaugeMaskImage.fillAmount = _targetAmount;

        if (PlayerManager.Instance.CurrentPlayerPoint < 8)
            _gaugeImage.color = new Color(1, 1, 1, 0.5f);
        else
            _gaugeImage.color = new Color(1, 1, 1, 1f);
    }

    private void HandlePlayerCircleSkillActiveEvent()
    {
        AudioManager.Instance.PlaySound(EAudioName.UltCharge);
        Blink(_activeColor, 0.5f)
            .AppendCallback(() => Pulse(_activeColor, (_activeColor + _defaultColor) / 2, 0.7f));

        //Canvas.ForceUpdateCanvases();
    }

    private void Pulse(Color color1, Color color2, float time)
    {
        _material.SetColor(_colorHash, color1);
        _material.DOColor(color2, _colorHash, time).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private Sequence Blink(Color color, float time)
    {
        Sequence sequence = DOTween.Sequence();
        _material.SetColor(_colorHash, _activeBlinkColor);
        _material.DOKill();
        sequence.Append(_material.DOColor(color, _colorHash, time).SetEase(Ease.OutQuad));
        return sequence;
    }

    private void HandleChangedPlayerCircleSkillGaugeEvent(float amount)
    {
        _targetAmount = amount / 100;
    }
}
