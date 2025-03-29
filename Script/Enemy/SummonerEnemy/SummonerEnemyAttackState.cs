using UnityEngine;
using YH.Animators;
using YH.Enemy;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class SummonerEnemyAttackState : EntityState
    {
        private SummonerEnemy _summonerEnemy;
        private EntityMover _mover;
        public SummonerEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _summonerEnemy = entity as SummonerEnemy;
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

            if (_summonerEnemy.AttackRangeInPlayer())
            {
                if (_summonerEnemy.lastAttackTime + _summonerEnemy.attackCooldownStat.Value < Time.time)
                {
                    _summonerEnemy.Summon();
                }
                else
                {
                    if (_summonerEnemy.enemyFSM[FSMState.Idle] == null)
                    {
                        _renderer.SetRotation(_summonerEnemy.PlayerDirection());
                        _mover.StopImmediately();
                    }
                    else
                    {
                        _summonerEnemy.ChangeState(_summonerEnemy.enemyFSM[FSMState.Idle]);
                    }
                }
            }
            else
            {
                _summonerEnemy.ChangeState(_summonerEnemy.enemyFSM[FSMState.Chase]);
            }
        }

    }
}

