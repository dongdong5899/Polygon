using Doryu.StatSystem;
using UnityEngine;
using UnityEngine.UIElements;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class EnemyChaseState : EntityState
    {
        private Enemy _enemy;

        private EntityMover _movement;
        private StatCompo _statCompo;
        

        public EnemyChaseState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy = entity as Enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _movement = _enemy.GetCompo<EntityMover>();
            _statCompo = _enemy.GetCompo<StatCompo>();
        }

        public override void Update()
        {
            base.Update();

            if (_enemy.AttackRangeInPlayer())
            {
                if (_enemy.lastAttackTime + _enemy.attackCooldownStat.Value < Time.time)
                {
                    _enemy.ChangeState(_enemy.enemyFSM[FSMState.Attack]);
                }
                else
                {
                    if (_enemy.enemyFSM[FSMState.Idle] == null)
                    {
                        _renderer.SetRotation(_enemy.PlayerDirection());
                        _movement.StopImmediately();
                    }
                    else
                    {
                        _enemy.ChangeState(_enemy.enemyFSM[FSMState.Idle]);
                    }
                }
            }
            else
            {
                Vector2 movement = _enemy.PlayerDirection();
                _movement.SetMovement(movement);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}
