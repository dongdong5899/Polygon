using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class DivisionEnemyDieState : EntityState
    {
        private DivisionEnemy _divisionEnemy;
        private EntityMover _mover;
        public DivisionEnemyDieState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _divisionEnemy = entity as DivisionEnemy;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            _divisionEnemy.Division();
            WaveManager.Instance.enemyList.Remove(_divisionEnemy);
        }

    }
}

