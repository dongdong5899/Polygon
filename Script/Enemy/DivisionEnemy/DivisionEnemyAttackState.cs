using DG.Tweening;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class DivisionEnemyAttackState : EntityState
    {
        private DivisionEnemy _divisionEnemy;

        private EntityMover _mover;

        public DivisionEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _divisionEnemy = entity as DivisionEnemy;
            _mover = _divisionEnemy.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();

            _divisionEnemy.lastAttackTime = Time.time;

            _mover.StopImmediately();

            Sequence sequence = DOTween.Sequence().SetAutoKill(true);
            sequence.Append(_renderer.transform.DORotate(new Vector3(0, 0, _renderer.transform.rotation.z + 360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.InBack));
            sequence.InsertCallback(0.1f, _divisionEnemy.Attack);
            sequence.AppendCallback(ChangeState);
        }

        private void ChangeState()
        {
            _divisionEnemy.ChangeState(_divisionEnemy.enemyFSM[FSMState.Chase]);
        }
    }
}

