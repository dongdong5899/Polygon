using ObjectPooling;
using UnityEngine;
using YH.FSM;

namespace YH.Enemy
{
    public class RollingShotEnemy : Enemy
    {
        [Header("RollingShotEnemy")]
        [SerializeField] private CircleCaster2D _defaultAttackCaster;
        [SerializeField] private float _bulletSpeed;
        [HideInInspector] public bool attackPose;
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
            attackPose = true;
            _attackRange = 1.5f;
        }

        public void CombatAttack()
        {
            if (_defaultAttackCaster.CheckCollision(out RaycastHit2D[] hits, whatIsTarget))
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.TryGetComponent(out HealthCompo health))
                    {
                        CameraManager.Instance.ShakeCamera(8, 8, 0.15f);
                        AudioManager.Instance.PlaySound(EAudioName.NearAttack);
                        health.ApplyDamage(_statCompo, Mathf.RoundToInt(damageStat.Value));
                        //Instantiate(attackEffect, health.transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            }
            _attackRange = 10f;
            attackPose = false;
        }
    }
}

