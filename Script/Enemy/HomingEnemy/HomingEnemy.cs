using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using UnityEngine.UIElements;
using YH.Entities;
using YH.FSM;

namespace YH.Enemy
{
    public class HomingEnemy : Enemy
    {
        [Header("HomingEnemy")]
        [SerializeField] private CircleCaster2D _nearAttackCaster;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _nearAttackRadius;
        [SerializeField] private float _nearAttackCooltime;
        [SerializeField] private SkillListSO _skillSetSO;
        private float _lastNearAttackTime = 0;
        
        private EntityMover _mover;

        protected override void Awake()
        {
            base.Awake();
            _mover = GetCompo<EntityMover>();
        }

        public void Attack()
        {
            float angle = 360f / 7;
            lastAttackTime = Time.time;
            for (int i = 0; i < 7; i++)
            {
                Homing homing = PoolManager.Instance.Pop(PoolingType.Homing) as Homing;
                homing.transform.SetPositionAndRotation(RotationTrm.position, RotationTrm.rotation * Quaternion.Euler(0, 0, RotationTrm.eulerAngles.z + 180 + angle * i));
                AudioManager.Instance.PlaySound(EAudioName.Fire);
                homing.Setting(this, whatIsTarget, PlayerManager.Instance.Player.transform, _bulletSpeed, Mathf.CeilToInt(damageStat.Value), 18);
            }
        }

        private Sequence _rotateSeq;
        protected override void Update()
        {
            base.Update();
            if (_lastNearAttackTime + _nearAttackCooltime > Time.time) return;
            if (_nearAttackCaster.CheckCollision(out RaycastHit2D[] hits, whatIsTarget))
            {
                _lastNearAttackTime = Time.time;
                if (_rotateSeq != null && _rotateSeq.IsActive()) _rotateSeq.Kill();
                _rotateSeq = DOTween.Sequence().SetAutoKill(true);
                _rotateSeq.Append(_renderer.transform.DORotate(new Vector3(0, 0, _renderer.transform.rotation.z + 720), 0.2f, RotateMode.FastBeyond360).SetEase(Ease.InElastic))
                    .InsertCallback(0.06f, () => {
                        foreach (RaycastHit2D hit in hits)
                        {
                            if (hit.transform.TryGetComponent(out Entity entity))
                            {
                                AudioManager.Instance.PlaySound(EAudioName.NearAttack);
                                entity.GetCompo<HealthCompo>().ApplyDamage(_statCompo, Mathf.CeilToInt(damageStat.Value * 3));
                                CameraManager.Instance.ShakeCamera(10, 10, 0.2f);
                            }
                        }
                    })
                    .AppendCallback(() => ChangeState(enemyFSM[FSMState.Chase]));
            }
        }
        protected override void HandleDieEvent()
        {
            DropItem dropItem = PoolManager.Instance.Pop(PoolingType.DropItem) as DropItem;
            dropItem.Setting(_skillSetSO.skillList[Random.Range(0, _skillSetSO.skillList.Count)], transform.position);
            base.HandleDieEvent();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _nearAttackRadius);
        }
    }
}
