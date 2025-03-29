using DG.Tweening;
using Doryu.StatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using YH.Entities;
using YH.FSM;

namespace YH.Players
{
    public class Player : Entity
    {
        public EntityStateListSO playerFSM;

        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        private StateMachine _stateMachine;
        private Dictionary<StateSO, EntityState> _stateDictionary;

        //public AnimParamSO comboCounterParam; 만약 근접공격이면 추가

        private EntityRenderer _renderer;

        [SerializeField] private float _dashCooltime = 2f;
        private float _currentCooltime;

        private Sequence _pointSeq;
        private float _currentPoint = 3;

        private float _lastCircleAttackTime = 0;
        private float _lastReproductionTime = 0;
        private float _reproductionDelay = 1f;
        private HealthCompo _health;
        private StatElement _healthStat;
        private StatElement _reproductionStat;
        private StatElement _damageStat;
        private StatElement _critical;

        [SerializeField] private GameObject _dirImage;
        [SerializeField] private DashSkillUI _dashSkillUI;
        [SerializeField] private SkillSO _skillSO;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine();
            _stateDictionary = new Dictionary<StateSO, EntityState>();
            _currentCooltime = Time.time;

            foreach (StateSO state in playerFSM.states)
            {
                try
                {
                    Type t = Type.GetType(state.className);
                    var playerState = Activator.CreateInstance(t, this, state.animParam) as EntityState;
                    _stateDictionary.Add(state, playerState);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{state.className} loading Error, Message : {ex.Message}");
                }
            }

            _renderer = GetCompo<EntityRenderer>();
        }

        private void OnDestroy()
        {
            PlayerInput.DashEvent -= HandleDash;
            _health.OnDieEvent -= HandleDieEvent;
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnChangedPlayerPointEvent -= HandlePointChangedEvent;
                PlayerManager.Instance.OnChangedPlayerLevelEvent -= HandleCircleSkillEvent;
                PlayerManager.Instance.OnCircleSkillEvent -= HandleCircleSkillEvent;
            }
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();

            PlayerInput.DashEvent += HandleDash;
            PlayerManager.Instance.OnChangedPlayerPointEvent += HandlePointChangedEvent;
            PlayerManager.Instance.OnChangedPlayerLevelEvent += HandleCircleSkillEvent;
            PlayerManager.Instance.OnCircleSkillEvent += HandleCircleSkillEvent;

            _health = GetCompo<HealthCompo>();
            _health.OnDieEvent += HandleDieEvent;

            _reproductionStat = GetCompo<StatCompo>().GetElement("Reproduction");
            _damageStat = GetCompo<StatCompo>().GetElement("Damage");
            _healthStat = GetCompo<StatCompo>().GetElement("Health");
            _critical = GetCompo<StatCompo>().GetElement("Critical");
        }

        private void HandleDieEvent()
        {
            _dirImage.SetActive(false);
            ChangeState(playerFSM[FSMState.Die]);

            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale / 200;

            PlayerInput.DashEvent -= HandleDash;
            _health.OnDieEvent -= HandleDieEvent;
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnChangedPlayerPointEvent -= HandlePointChangedEvent;
                PlayerManager.Instance.OnChangedPlayerLevelEvent -= HandleCircleSkillEvent;
                PlayerManager.Instance.OnCircleSkillEvent -= HandleCircleSkillEvent;
            }
        }

        private void HandleCircleSkillEvent(int level)
        {
            _healthStat.AddModify("Level" + level, 1000);
            _damageStat.AddModify("Level" + level, 250);
            if (level <= 11)
                _critical.AddModify("Level" + level, 1);
        }

        private void HandleCircleSkillEvent(bool isUse)
        {
            StatCompo statCompo = GetCompo<StatCompo>();
            if (isUse)
            {
                statCompo.GetElement("Speed").AddModifyPercent("CircleSkill", 100f);
                statCompo.GetElement("Damage").AddModifyPercent("CircleSkill", 50f);
                statCompo.GetElement("Reproduction").AddModifyPercent("CircleSkill", 1000f);
                statCompo.GetElement("Critical").AddModify("CircleSkill", 100f);
                _renderer.Blink(1);
            }
            else
            {
                statCompo.GetElement("Speed").RemoveModifyPercent("CircleSkill");
                statCompo.GetElement("Damage").RemoveModifyPercent("CircleSkill");
                statCompo.GetElement("Reproduction").RemoveModifyPercent("CircleSkill");
                statCompo.GetElement("Critical").RemoveModify("CircleSkill");
                _renderer.Blink(0);
            }
        }

        private void HandlePointChangedEvent(int pointCount)
        {
            if (_pointSeq != null && _pointSeq.IsActive()) _pointSeq.Kill();
            _pointSeq = DOTween.Sequence();
            float startValue = _currentPoint;
            _pointSeq.Append(DOTween.To(
                () => startValue,
                value =>
                {
                    _currentPoint = value;
                    _renderer.SetPolygonShape(value, Color.white);
                },
                pointCount, 0.7f).SetEase(Ease.OutCirc));

        }

        private void HandleDash()
        {
            if (PlayerInput.InputDirection.sqrMagnitude > 0.05f && _currentCooltime < Time.time)
            {
                ChangeState(playerFSM[FSMState.Dash]);
                _currentCooltime = Time.time + _dashCooltime;
            }
        }


        private void Start()
        {
            _stateMachine.Initialize(GetState(playerFSM[FSMState.Idle]));
            _currentPoint = PlayerManager.Instance.CurrentPlayerPoint;
        }

        public void ChangeState(StateSO newState) => _stateMachine.ChangeState(GetState(newState));
        private EntityState GetState(StateSO stateSo) => _stateDictionary.GetValueOrDefault(stateSo);

        private void Update()
        {
            if (GameManager.Instance.IsGameOver) return;

            _stateMachine.UpdateStateMachine();

            _dashSkillUI.SetDashUIAmount(Mathf.Clamp((_currentCooltime - Time.time) / _dashCooltime, 0f, 1f));

            SetRotation();

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                DropItem item = PoolManager.Instance.Pop(ObjectPooling.PoolingType.DropItem) as DropItem;
                item.Setting(_skillSO, transform.position);
            }

            if (_lastReproductionTime + _reproductionDelay < Time.time)
            {
                _lastReproductionTime = Time.time;
                _health.ApplyRecovery(Mathf.CeilToInt(_reproductionStat.Value));
            }

            if (PlayerManager.Instance.isUseCircleSkill &&
                _lastCircleAttackTime + 1 < Time.time)
            {
                _lastCircleAttackTime = Time.time;

                float rotationDeg = 360f / 15;
                for (int i = 0; i < 15; i++)
                {
                    float angle = rotationDeg * i + _renderer.Direction;

                    Vector3 dir = new Vector3(
                        Mathf.Cos((angle + 90) * Mathf.Deg2Rad),
                        Mathf.Sin((angle + 90) * Mathf.Deg2Rad),
                         0).normalized;
                    dir *= transform.localScale.x;

                    Vector3 position = transform.position + dir;

                    int damage = Mathf.CeilToInt(_damageStat.Value);
                    Projectile projectile
                        = PoolManager.Instance.Pop(ObjectPooling.PoolingType.PlayerProjectile) as Projectile;
                    projectile.Setting(PlayerManager.Instance.Player, 1 << LayerMask.NameToLayer("Enemy"), 15, damage);
                    projectile.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, angle));
                    projectile.transform.localScale = Vector3.one;
                }
            }
        }

        private void SetRotation()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(PlayerInput.MousePos);
            mouseWorldPos.z = 0;
            Vector3 mouseDir = (mouseWorldPos - transform.position).normalized;
            _renderer.SetRotation(mouseDir, true);
        }
    }
}
