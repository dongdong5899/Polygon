using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;

namespace YH.Enemy
{
    public class SummonerEnemy : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void Summon()
        {
            lastAttackTime = Time.time;
            _healthCompo.ApplyDamage(_statCompo, 100, true, false);
            GameObject enemy = PoolManager.Instance.Pop(PoolingType.ChildEnemy).GameObject;
            Vector2 playerDir = (PlayerManager.Instance.Player.transform.position - transform.position).normalized;
            enemy.transform.position = transform.position + (Vector3)playerDir;
        }

       

    }
}

