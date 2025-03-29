using Doryu.StatSystem;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using YH.Entities;
using YH.FSM;
using YH.Players;

namespace YH.Enemy
{
    public class SelfBombEnemy : Enemy
    {
        [field:SerializeField] public CircleCaster2D defaultAttackCaster;
        [SerializeField] private StatElementSO delayTimeSO;
        public StatElement delayTime { get;private set; }


        protected override void Awake()
        {
            base.Awake();
            delayTime = _statCompo.GetElement(delayTimeSO);
        }

        public void Attack()
        {

            if (defaultAttackCaster.CheckCollision(out RaycastHit2D[] hits, whatIsTarget))
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.TryGetComponent(out HealthCompo health))
                    {
                        CameraManager.Instance.ShakeCamera(6, 6, 0.15f);
                        health.ApplyDamage(_statCompo, Mathf.RoundToInt(damageStat.Value));
                        //Instantiate(attackEffect, health.transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            }

            AudioManager.Instance.PlaySound(EAudioName.Bomb);
            GetCompo<HealthCompo>().ApplyDamage(_statCompo, 10000, isTextVisible: false);
        }
    }

}

