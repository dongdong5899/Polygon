using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class CombatEnemyDieState : EntityState
    {
        private CombatEnemy _enemy_6;
        private EntityMover _mover;
        public CombatEnemyDieState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _enemy_6 = entity as CombatEnemy;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            BombDisplay bombDisplay = PoolManager.Instance.Pop(PoolingType.BombDisplay) as BombDisplay;
            bombDisplay.Setting(_enemy_6.secondAttackCaster.radius, _enemy_6.transform);

            Sequence sequence = DOTween.Sequence().SetAutoKill(true);
            sequence.Append(_renderer.transform.DORotate
                (new Vector3(0, 0, _renderer.transform.rotation.z + 720), _enemy_6.delayTime.Value, RotateMode.FastBeyond360).SetEase(Ease.InElastic));
            sequence.AppendCallback(_enemy_6.BombAttack);
            sequence.AppendCallback(()=> {
                PoolManager.Instance.Push(_enemy_6);
                _enemy_6.DOKill();
            });
            WaveManager.Instance.enemyList.Remove(_enemy_6);
        }


    }
}


