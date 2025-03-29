using ObjectPooling;
using UnityEngine;


namespace YH.Enemy
{
    public class BigBulletEnemy : Enemy
    {
        [Header("Enemy_4")]
        [SerializeField] private float _bulletSpeed;

        public void Attack()
        {
            lastAttackTime = Time.time;
            Projectile arrow = PoolManager.Instance.Pop(PoolingType.EnemyProjectile) as Projectile;
            arrow.SetScale(2);
            arrow.transform.SetPositionAndRotation(RotationTrm.position, RotationTrm.rotation);
            //AudioManager.Instance.PlaySound(EAudioName.Fire);
            arrow.Setting(this, whatIsTarget, _bulletSpeed, Mathf.CeilToInt(damageStat.Value));
        }
    }
}

