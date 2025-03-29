using Doryu.StatSystem;
using UnityEngine;
using ObjectPooling;

namespace YH.Enemy
{
    public class DefaultEnemy : Enemy
    {
        [Header("DefaultEnemy")]
        [SerializeField] private float _bulletSpeed;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Attack()
        {
            lastAttackTime = Time.time;
            Projectile arrow = PoolManager.Instance.Pop(PoolingType.EnemyProjectile) as Projectile;
            arrow.transform.SetPositionAndRotation(RotationTrm.position, RotationTrm.rotation);
            AudioManager.Instance.PlaySound(EAudioName.Fire);
            arrow.Setting(this, whatIsTarget, _bulletSpeed, Mathf.CeilToInt(damageStat.Value));
        }
    }
}
