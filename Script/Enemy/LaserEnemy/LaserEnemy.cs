using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;
using YH.Entities;

namespace YH.Enemy
{
    public class LaserEnemy : Enemy
    {
        [SerializeField] private StatElementSO delayTimeSO;
        public StatElement delayTime { get; private set; }
        private Vector3 _playerDir;

        protected override void Awake()
        {
            base.Awake();
            delayTime = _statCompo.GetElement(delayTimeSO);
        }

        public void Attack(Transform _displaytrm)
        {
            Laser laser = PoolManager.Instance.Pop(PoolingType.EnemyLaser) as Laser;
            _renderer.SetRotation(_playerDir);
            laser.transform.SetPositionAndRotation(_displaytrm.position, _displaytrm.rotation);
            laser.Setting(this, whatIsTarget, Mathf.CeilToInt(damageStat.Value));
            lastAttackTime = Time.time;
            lastAttackTime = Time.time;
        }
    }
}

