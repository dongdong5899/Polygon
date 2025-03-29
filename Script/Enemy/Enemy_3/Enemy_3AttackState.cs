using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class Enemy_3AttackState : EntityState
    {
        private Enemy_3 _enemy_3;
        private EntityMover _mover;
        private StatCompo _statCompo;
        
        public Enemy_3AttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy_3 = entity as Enemy_3;
            _statCompo = entity.GetCompo<StatCompo>();
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            BombDisplay bombDisplay = PoolManager.Instance.Pop(PoolingType.BombDisplay) as BombDisplay;
            bombDisplay.Setting(_enemy_3.defaultAttackCaster.radius,_enemy_3.transform);

            Sequence sequence = DOTween.Sequence();
            _enemy_3.chargingSkill = sequence;
            sequence.Append(_renderer.transform.DORotate
                (new Vector3(0, 0, _renderer.transform.rotation.z + 720), _enemy_3.delayTime.Value, RotateMode.FastBeyond360).SetEase(Ease.InElastic));
            sequence.AppendCallback(_enemy_3.Attack);
        }

        public override void Update()
        {
            base.Update();
            if (!_enemy_3.enabled)
            {
                _enemy_3.DOKill();   
            }
        }
    }
}

