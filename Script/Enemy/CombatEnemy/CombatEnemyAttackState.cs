using System;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using DG.Tweening;
using Doryu.StatSystem;

namespace YH.Enemy
{
    public class CombatEnemyAttackState : EntityState
    {
        private CombatEnemy _enemy_6;

        private EntityMover _mover;

        public CombatEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy_6 = entity as CombatEnemy;
            _mover = _enemy_6.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();

            _enemy_6.lastAttackTime = Time.time;

            _mover.StopImmediately();

            Sequence sequence = DOTween.Sequence().SetAutoKill(true);
            sequence.Append(_renderer.transform.DORotate(new Vector3(0, 0, _renderer.transform.rotation.z + 360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.InBack));
            sequence.InsertCallback(0.1f, _enemy_6.Attack);
            sequence.AppendCallback(ChangeState);
        }

        private void ChangeState()
        {
            _enemy_6.ChangeState(_enemy_6.enemyFSM[FSMState.Chase]);
        }
    }
}

