using Doryu.StatSystem;
using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;

namespace YH.Enemy
{
    public class BurstEnemy : Enemy
    {
        [Header("Burst_Enemy")]
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _bulletCount;
        [SerializeField] private float _bulletDelay;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Attack()
        {
            lastAttackTime = Time.time;
            StartCoroutine(BurstFire());
        }

        private IEnumerator BurstFire()
        {
            for (int i = 0; i < _bulletCount; i++)
            {
                Projectile arrow = PoolManager.Instance.Pop(PoolingType.EnemyProjectile) as Projectile;
                arrow.transform.SetPositionAndRotation(RotationTrm.position, RotationTrm.rotation);
                AudioManager.Instance.PlaySound(EAudioName.Fire);
                arrow.Setting(this, whatIsTarget, _bulletSpeed, Mathf.CeilToInt(damageStat.Value));
                yield return new WaitForSeconds(_bulletDelay);
            }
            lastAttackTime = Time.time;
        }
    }
}

