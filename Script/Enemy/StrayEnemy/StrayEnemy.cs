using ObjectPooling;
using UnityEngine;

namespace YH.Enemy
{
    public class StrayEnemy : Enemy
    {
        [Header("StrayEnemy")]
        [SerializeField] private float _bulletSpeed;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Attack()
        {
            lastAttackTime = Time.time;
            StrayBullet arrow = PoolManager.Instance.Pop(PoolingType.StrayBullet) as StrayBullet;
            arrow.transform.SetPositionAndRotation(RotationTrm.position, RotationTrm.rotation);
            AudioManager.Instance.PlaySound(EAudioName.Fire);
            arrow.Setting(this, whatIsTarget, _bulletSpeed, Mathf.CeilToInt(damageStat.Value));
        }
    }

}
