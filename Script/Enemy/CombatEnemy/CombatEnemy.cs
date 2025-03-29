using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;

namespace YH.Enemy
{
    public class CombatEnemy : Enemy
    {
        [SerializeField] private CircleCaster2D _defaultAttackCaster;
        [field: SerializeField] public CircleCaster2D secondAttackCaster;
        [SerializeField] private StatElementSO delayTimeSO;
        [SerializeField] private SkillListSO _skillSetSO;

        public StatElement delayTime { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            delayTime = _statCompo.GetElement(delayTimeSO);
        }

        public void Attack()
        {
            if (_defaultAttackCaster.CheckCollision(out RaycastHit2D[] hits, whatIsTarget))
            {
                if (hits[0].transform.TryGetComponent(out HealthCompo health))
                {
                    CameraManager.Instance.ShakeCamera(8, 8, 0.15f);
                    AudioManager.Instance.PlaySound(EAudioName.NearAttack);
                    health.ApplyDamage(_statCompo, Mathf.RoundToInt(damageStat.Value));
                    //Instantiate(attackEffect, health.transform.position, Quaternion.identity);
                }
            }
            else
            {
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            }
        }

        public void BombAttack()
        {
            AudioManager.Instance.PlaySound(EAudioName.Bomb);
            if (secondAttackCaster.CheckCollision(out RaycastHit2D[] hits, whatIsTarget))
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.TryGetComponent(out HealthCompo health))
                    {
                        CameraManager.Instance.ShakeCamera(8, 8, 0.15f);
                        health.ApplyDamage(_statCompo, Mathf.RoundToInt(damageStat.Value));
                        //Instantiate(attackEffect, health.transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            }
        }

        protected override void HandleDieEvent()
        {
            DropItem dropItem = PoolManager.Instance.Pop(PoolingType.DropItem) as DropItem;
            dropItem.Setting(_skillSetSO.skillList[Random.Range(0, _skillSetSO.skillList.Count)], transform.position);
            base.HandleDieEvent();
        }
    }


}
