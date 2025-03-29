using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using System;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class LaserEnemyAttackState : EntityState
    {
        private LaserEnemy _enemy_5;
        private EntityMover _mover;
        private StatCompo _statCompo;

        public LaserEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy_5 = entity as LaserEnemy;
            _statCompo = entity.GetCompo<StatCompo>();
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            LaserDisplay laser = PoolManager.Instance.Pop(PoolingType.LaserDisplay) as LaserDisplay;
            laser.transform.SetPositionAndRotation(_enemy_5.RotationTrm.position + _enemy_5.RotationTrm.forward, _enemy_5.RotationTrm.rotation);
            laser.Setting(_enemy_5.RotationTrm);

            Sequence sequence = DOTween.Sequence();
            _enemy_5.chargingSkill = sequence;
            sequence.Append(_renderer.transform.DORotate
                (new Vector3(0, 0, _renderer.transform.rotation.z + 1080), _enemy_5.delayTime.Value, RotateMode.FastBeyond360).SetEase(Ease.OutBounce));
            sequence.InsertCallback(1.6f, () =>
            {
                laser.FixAim();
                AudioManager.Instance.PlaySound(EAudioName.Laser);
            });
            sequence.InsertCallback(2f, () =>
            {
                _enemy_5.Attack(laser.transform);
            });
            sequence.AppendCallback(ChangeState);
        }

        private void ChangeState()
        {
            _enemy_5.lastAttackTime = Time.time;
            _enemy_5.ChangeState(_enemy_5.enemyFSM[FSMState.Chase]);
        }

        public override void Update()
        {
            base.Update();
            if (_enemy_5.enabled)
            {
                _enemy_5.DOKill();
            }
        }
    }
}

