using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class SelfBombEnemyAttackState : EntityState
    {
        private SelfBombEnemy _enemy;
        private EntityMover _mover;
        private StatCompo _statCompo;
        
        public SelfBombEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy = entity as SelfBombEnemy;
            _statCompo = entity.GetCompo<StatCompo>();
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            BombDisplay bombDisplay = PoolManager.Instance.Pop(PoolingType.BombDisplay) as BombDisplay;
            bombDisplay.Setting(_enemy.defaultAttackCaster.radius,_enemy.transform);

            Sequence sequence = DOTween.Sequence();
            _enemy.chargingSkill = sequence;
            sequence.Append(_renderer.transform.DORotate
                (new Vector3(0, 0, _renderer.transform.rotation.z + 720), _enemy.delayTime.Value, RotateMode.FastBeyond360).SetEase(Ease.InElastic));
            sequence.AppendCallback(_enemy.Attack);
        }

        public override void Update()
        {
            base.Update();
            if (!_enemy.enabled)
            {
                _enemy.DOKill();   
            }
        }
    }
}

