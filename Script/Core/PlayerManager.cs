using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YH.Players;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private Player _player;
    public Player Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindFirstObjectByType<Player>();
                if (_player == null)
                    Debug.Log("Player가 없단다");
            }
            return _player;
        }
    }

    public float _currentPlayerCircleSkillGauge = 0;
    public float CurrentPlayerCircleSkillGauge
    {
        get => _currentPlayerCircleSkillGauge;
        private set
        {
            _currentPlayerCircleSkillGauge = value;
            OnChangedPlayerCircleSkillGaugeEvent?.Invoke(value);
        }
    }
    public event Action<float> OnChangedPlayerCircleSkillGaugeEvent;
    public event Action OnPlayerCircleSkillActiveEvent;
    public bool isOnCircleSkill = false;
    public bool isUseCircleSkill = false;

    public int CurrentPlayerReShapeCount { get; private set; } = 0;
    public event Action<int> OnPlayerReShapeEvent;

    public int CurrentPlayerLevel { get; private set; } = 1;
    public event Action<int> OnChangedPlayerLevelEvent;

    private int _currentPlayerPoint = 3;
    public int CurrentPlayerPoint
    {
        get => _currentPlayerPoint;
        private set
        {
            if (isUseCircleSkill == false && _currentPlayerPoint != value)
                OnChangedPlayerPointEvent?.Invoke(value);
            _currentPlayerPoint = value;
        }
    }
    public event Action<int> OnChangedPlayerPointEvent;

    public int CurrentExp { get; private set; } = 0;
    public event Action<int> OnExpChangedEvent;
    public int MaxExp { get; private set; } = 100;

    [SerializeField] private PlayerInputSO _playerInput;


    public event Action<bool> OnCircleSkillEvent;

    [SerializeField] private Vector2Int _minMaxPlayerLevel = new Vector2Int(1, 20);
    [SerializeField] private Vector2Int _minMaxPlayerPoint = new Vector2Int(3, 8);

    [SerializeField] private Volume _volume;

    public void ReShape()
    {
        CurrentPlayerPoint = _minMaxPlayerPoint.x;
        CurrentPlayerReShapeCount++;
        OnPlayerReShapeEvent?.Invoke(CurrentPlayerReShapeCount);
    }

    public void AddCircleSkillGauge(float addGauge)
    {
        if (isUseCircleSkill || isOnCircleSkill) return;

        CurrentPlayerCircleSkillGauge += addGauge;
        if (isOnCircleSkill == false && CurrentPlayerCircleSkillGauge >= 100)
        {
            isOnCircleSkill = true;
            CurrentPlayerCircleSkillGauge = 100;
            OnPlayerCircleSkillActiveEvent?.Invoke();
        }
    }

    public void UseCircleSkill()
    {
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.overrideState = true;
            DOTween.To(() => 0f, value => vignette.intensity.value = value, 0.4f, 0.2f);
        }
        if (_volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.overrideState = true;
            DOTween.To(() => 0f, value => chromaticAberration.intensity.value = value, 0.15f, 0.2f);
        }
        isOnCircleSkill = false;
        isUseCircleSkill = true;
        OnCircleSkillEvent?.Invoke(true);
        OnChangedPlayerPointEvent?.Invoke(30);
    }

    public void CancelCircleSkill()
    {
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            DOTween.To(() => 0.4f, value => vignette.intensity.value = value, 0f, 0.2f);
        }
        if (_volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            DOTween.To(() => 0.15f, value => chromaticAberration.intensity.value = value, 0f, 0.2f);
        }
        isOnCircleSkill = false;
        isUseCircleSkill = false;
        OnCircleSkillEvent?.Invoke(false);
        CurrentPlayerCircleSkillGauge = Mathf.Clamp(CurrentPlayerCircleSkillGauge, 0f, 50f);
        OnChangedPlayerPointEvent?.Invoke(CurrentPlayerPoint);
    }

    private void LevelUp()
    {
        CurrentPlayerLevel++;
        if (CurrentPlayerLevel > _minMaxPlayerLevel.y)
            CurrentPlayerLevel = _minMaxPlayerLevel.y;
        else
        {
            OnChangedPlayerLevelEvent?.Invoke(CurrentPlayerLevel);

            CurrentPlayerPoint++;
            if (CurrentPlayerPoint < _minMaxPlayerPoint.x || CurrentPlayerPoint > _minMaxPlayerPoint.y)
                CurrentPlayerPoint = _minMaxPlayerPoint.y;
        }

        MaxExp += (int)(MathF.Log(2, CurrentPlayerLevel) * 100);
    }
    public void AddExp(int exp)
    {
        CurrentExp += exp;
        if (CurrentExp > MaxExp)
        {
            CurrentExp -= MaxExp;
            LevelUp();
        }
        OnExpChangedEvent?.Invoke(CurrentExp);
    }

    private void Awake()
    {
        _playerInput.CircleSkillEvent += HandleCircleSkillEvent;
    }

    private void OnDestroy()
    {
        _playerInput.CircleSkillEvent -= HandleCircleSkillEvent;
    }

    private void HandleCircleSkillEvent()
    {
        if (isUseCircleSkill)
            CancelCircleSkill();
        else if (isOnCircleSkill && CurrentPlayerPoint == 8)
            UseCircleSkill();
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    LevelUp();
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    AddCircleSkillGauge(10);
        //}

        if (isUseCircleSkill)
        {
            CurrentPlayerCircleSkillGauge -= Time.deltaTime * 100f / 15;
            if (CurrentPlayerCircleSkillGauge < 0)
            {
                CurrentPlayerCircleSkillGauge = 0;
                CancelCircleSkill();
            }
        }
    }
}
