using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class PlayerPointText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _whitePointText;
    [SerializeField] private TextMeshProUGUI _blackPointText;

    private void Start()
    {
        PlayerManager.Instance.OnChangedPlayerPointEvent += HandleChangedPlayerPointEvent;
        PlayerManager.Instance.OnCircleSkillEvent += HandleCircleSkillEvent;
        HandleChangedPlayerPointEvent(PlayerManager.Instance.CurrentPlayerPoint);
    }

    private void HandleCircleSkillEvent(bool value)
    {
        _whitePointText.transform.DOKill();
        _whitePointText.transform.DOLocalRotate(new Vector3(0, 0, 360f), 0.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        _blackPointText.transform.DOKill();
        _blackPointText.transform.DOLocalRotate(new Vector3(0, 0, 360f), 0.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        if (value)
        {
            _whitePointText.text = "¡Ä";
            _blackPointText.text = "¡Ä";
        }
        else
            HandleChangedPlayerPointEvent(PlayerManager.Instance.CurrentPlayerPoint);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnChangedPlayerPointEvent -= HandleChangedPlayerPointEvent;
            PlayerManager.Instance.OnCircleSkillEvent -= HandleCircleSkillEvent;
        }
    }

    private void HandleChangedPlayerPointEvent(int value)
    {
        if (PlayerManager.Instance.isUseCircleSkill) return;
        string text = value.ToString();
        _whitePointText.text = text;
        _blackPointText.text = text;
    }
}
