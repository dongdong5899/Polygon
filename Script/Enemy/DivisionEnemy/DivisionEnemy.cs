using Doryu.StatSystem;
using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;


namespace YH.Enemy
{
    public class DivisionEnemy : Enemy
    {
        [SerializeField] private CircleCaster2D _defaultAttackCaster;

        public StatElement delayTime { get; private set; }

        public void Attack()
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
        }

        public void Division()
        {
            StartCoroutine(DivisionCoroutine());
        }

        private IEnumerator DivisionCoroutine()
        {
            for (int i = 0; i < 2; i++)
            {
                Transform enemy = PoolManager.Instance.Pop(PoolingType.ChildEnemy).GameObject.transform;
                enemy.position = transform.position;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}


