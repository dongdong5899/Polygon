using DG.Tweening;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class RollingShotEnemyAttackState : EntityState
    {
        private RollingShotEnemy _rollingEnemy;
        private EntityMover _mover;
        public RollingShotEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _rollingEnemy = entity as RollingShotEnemy;
            _mover = _rollingEnemy.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();

            _rollingEnemy.lastAttackTime = Time.time;

            _mover.StopImmediately();

            if (_rollingEnemy.attackPose)
            {
                Sequence sequence = DOTween.Sequence().SetAutoKill(true);
                sequence.Append(_renderer.transform.DORotate(new Vector3(0, 0, _renderer.transform.rotation.z + 360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.InBack));
                sequence.InsertCallback(0.1f, _rollingEnemy.CombatAttack);
                sequence.AppendCallback(ChangeState);
            }
            else
            {
                _rollingEnemy.Attack();
                ChangeState();
            }
            
        }

        private void ChangeState()
        {
            _rollingEnemy.ChangeState(_rollingEnemy.enemyFSM[FSMState.Chase]);
        }

        
    }
}


