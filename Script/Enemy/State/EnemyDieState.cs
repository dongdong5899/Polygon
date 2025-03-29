using DG.Tweening;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class EnemyDieState : EntityState
    {
        private float _dieTime;
        private float _dissolveTime = 1.5f;

        private Enemy _enemy;

        public EnemyDieState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy = entity as Enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _dieTime = Time.time;
            //_entity.GetCompo<EntityRenderer>().Dissolve(_dissolveTime);
            _enemy.GetCompo<EntityMover>()?.StopImmediately();
            PoolManager.Instance.Push(_enemy);
            WaveManager.Instance.enemyList.Remove(_enemy);
            _enemy.DOKill();
            if(_enemy.chargingSkill!=null)
            {
                _enemy.chargingSkill.Kill();
                _enemy.chargingSkill = null;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            //if (_dieTime + _dissolveTime < Time.time)
            //{
            //}
        }
    }

}
