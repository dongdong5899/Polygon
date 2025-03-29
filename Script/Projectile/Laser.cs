using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;
using YH.Entities;

public class Laser : MonoBehaviour,IPoolable
{
    private Transform _visualTrm;
    private BoxCaster2D _boxCaster;
    private LayerMask _whatIsTarget;
    private int _damage;
    private Transform _parent;
    [SerializeField] private float _lifetime;
    private float _popTime;
    private float _invincibleTime = 0.5f;
    private float _dieTime;
    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    private Entity _entity;

    private void Awake()
    {
        _boxCaster = GetComponent<BoxCaster2D>();
        _visualTrm = transform.GetChild(0);
    }

    public void Init()
    {
        _popTime = Time.time;
        transform.localScale = new Vector3(1,100);
    }

    public void Setting(Entity entity, LayerMask whatIsTarget, int damage)
    {
        _entity = entity;
        _whatIsTarget = whatIsTarget;
        _damage = damage;
    }


    private void Update()
    {

        if (_lifetime + _popTime < Time.time)
        {
            if (PoolType == PoolingType.PlayerLaser)
            {
                _dieTime = 1.5f;
            }
            else if (PoolType == PoolingType.EnemyLaser)
            {
                _dieTime = 1.2f;
            }

            float width = 1 + (_lifetime + _popTime - Time.time)* _dieTime;
            if (width > 0)
                transform.localScale = new Vector3(width, 100, 1);
            else
                PoolManager.Instance.Push(this);
        }




        if (_boxCaster.CheckCollision(out RaycastHit2D[] hits, _whatIsTarget))
        {
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<Entity>(out Entity entity))
                {
                    HealthCompo health = entity.GetCompo<HealthCompo>();
                    if(health.LastAttackTime + _invincibleTime < Time.time)
                    {
                        health.ApplyDamage(_entity.GetCompo<StatCompo>(), _damage);
                        health.LastAttackTime = Time.time;
                    }
                    CameraManager.Instance.ShakeCamera(8, 8, 0.1f);
                }
            }
        }
        else
        {
            CameraManager.Instance.ShakeCamera(4, 4, 0.1f);
        }
    }
}
