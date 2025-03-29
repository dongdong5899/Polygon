using YH.Entities;
using YH.FSM;
using YH.Players;
using System.Collections.Generic;
using System;
using UnityEngine;
using Doryu.StatSystem;
using ObjectPooling;
using DG.Tweening;

namespace YH.Enemy
{
    public class Enemy : Entity, IPoolable
    {
        public EntityStateListSO enemyFSM;

        private StateMachine _stateMachine;
        private Dictionary<StateSO, EntityState> _stateDictionary;

        [SerializeField] private int _exp;
        [SerializeField] protected float _attackRange;
        private Player _player;
        protected EntityRenderer _renderer;
        protected StatCompo _statCompo;
        protected HealthCompo _healthCompo;
        public Transform RotationTrm { get; private set; }
        [HideInInspector] public float lastAttackTime;

        [field: SerializeField] private StatElementSO _damageSO, _attackCooldownSO;
        public StatElement damageStat { get; private set; }
        public StatElement attackCooldownStat { get; private set; }

        public Sequence chargingSkill;

        public GameObject GameObject { get => gameObject; set { } }
        [field: SerializeField] public PoolingType PoolType { get; set; }

        public LayerMask whatIsTarget;

        //public AnimParamSO comboCounterParam; 만약 근접공격이면 추가

        protected override void Awake()
        {
            _player = PlayerManager.Instance.Player;
            base.Awake();
            #region StateMachine
            _stateMachine = new StateMachine();
            _stateDictionary = new Dictionary<StateSO, EntityState>();

            foreach (StateSO state in enemyFSM.states)
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
            #endregion

            _renderer = GetCompo<EntityRenderer>();
            RotationTrm = _renderer.transform.parent;
            GetCompo<HealthCompo>().OnDieEvent += HandleDieEvent;

            _statCompo = GetCompo<StatCompo>();
            _healthCompo = GetCompo<HealthCompo>();
            damageStat = _statCompo.GetElement(_damageSO);
            attackCooldownStat = _statCompo.GetElement(_attackCooldownSO);
        }

        protected virtual void HandleDieEvent()
        {
            WaveManager.Instance.RemoveEnemyCount();
            PlayerManager.Instance.AddExp(_exp);
            Transform effect = PoolManager.Instance.Pop(PoolingType.EnemyDieEffect).GameObject.transform;
            effect.position = transform.position;
            ChangeState(enemyFSM[FSMState.Die]);
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
        }

        private void Start()
        {
            if (enemyFSM[FSMState.Chase] != null)
                _stateMachine.Initialize(GetState(enemyFSM[FSMState.Chase]));
            else
                _stateMachine.Initialize(GetState(enemyFSM[FSMState.Idle]));
        }

        public void ChangeState(StateSO newState) => _stateMachine.ChangeState(GetState(newState));
        private EntityState GetState(StateSO stateSo) => _stateDictionary.GetValueOrDefault(stateSo);
        public bool AttackRangeInPlayer() => Vector3.Distance(_player.transform.position, transform.position) < _attackRange;
        public Vector3 PlayerDirection() => (_player.transform.position - transform.position).normalized;

        protected virtual void Update()
        {
            _stateMachine.UpdateStateMachine();

            _renderer.SetRotation(PlayerDirection());

            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    _healthCompo.ApplyDamage(10000, isTextVisible: false);
            //}

        }


        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }

        public void Init()
        {
            _healthCompo.Resurrection();
            _stateMachine.Initialize(GetState(enemyFSM[FSMState.Chase]));
        }
    }
}
