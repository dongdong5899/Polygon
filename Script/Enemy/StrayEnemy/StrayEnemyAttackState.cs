using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class StrayEnemyAttackState : EntityState
    {
        private StrayEnemy _enemy;
        private EntityMover _mover;
        public StrayEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy = entity as StrayEnemy;
            _mover = entity.GetCompo<EntityMover>();
        }
        
        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
        }

        public override void Update()
        {
            base.Update();

            if (_enemy.AttackRangeInPlayer())
            {
                if (_enemy.lastAttackTime + _enemy.attackCooldownStat.Value < Time.time)
                {
                    _enemy.Attack();
                }
                else
                {
                    if (_enemy.enemyFSM[FSMState.Idle] == null)
                    {
                        _renderer.SetRotation(_enemy.PlayerDirection());
                        _mover.StopImmediately();
                    }
                    else
                    {
                        _enemy.ChangeState(_enemy.enemyFSM[FSMState.Idle]);
                    }
                }
            }
            else
            {
                _enemy.ChangeState(_enemy.enemyFSM[FSMState.Chase]);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

