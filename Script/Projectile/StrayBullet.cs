using Doryu.StatSystem;
using ObjectPooling;
using UnityEngine;
using YH.Entities;

public class StrayBullet : MonoBehaviour,IPoolable
{
    private Transform _visualTrm;
    private Caster2D _caster;
    private LayerMask _whatIsTarget;
    private float _speed;
    private int _damage;
    private bool _isEnable;


    [SerializeField] private Caster2D _strayCaster;
    [SerializeField] private PoolingType _effectType;
    [SerializeField] private float _rotate = 10f;
    [SerializeField] private float _lifetime = 3f;
    private float _popTime;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    private Entity _entity;

    private Vector3 _defaultScale, _casterScale;

    private void Awake()
    {
        _caster = GetComponent<Caster2D>();
        _visualTrm = transform.GetChild(0);
        _defaultScale = transform.localScale;
        _casterScale = _caster.offset;
    }

    public void Init()
    {
        _isEnable = true;
        _popTime = Time.time;
        transform.localScale = _defaultScale;
        _caster.offset = _casterScale;
    }

    public void Setting(Entity entity, LayerMask whatIsTarget, float speed, int damage)
    {
        _entity = entity;
        _whatIsTarget = whatIsTarget;
        _speed = speed;
        _damage = damage;
    }

    public void SetScale(float multiple)
    {
        transform.localScale *= multiple;
        _caster.offset *= multiple;
    }

    private void FixedUpdate()
    {
        if (_lifetime + _popTime < Time.time)
        {
            _isEnable = false;
            Die();
        }

        if (_isEnable == false) return;

        _visualTrm.Rotate(Vector3.forward * _rotate);

        Vector3 movement = transform.up * (Time.fixedDeltaTime * _speed);
        if (_caster.CheckCollision(out RaycastHit2D[] hits, _whatIsTarget, movement))
        {
            if (hits[0].transform.TryGetComponent(out Entity entity))
            {
                Die();
                CameraManager.Instance.ShakeCamera(8, 8, 0.1f);
            }
            _isEnable = false;
            transform.position += transform.up * hits[0].distance;
        }
        else
        {
            transform.position += movement;
        }
    }

    private void Die()
    {
        if (_strayCaster.CheckCollision(out RaycastHit2D[] hits,_whatIsTarget))
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out HealthCompo health))
                {
                    CameraManager.Instance.ShakeCamera(6, 6, 0.15f);
                    health.ApplyDamage(_entity.GetCompo<StatCompo>(), _damage);
                }
            }
        }
        Transform effect = PoolManager.Instance.Pop(_effectType).GameObject.transform;
        effect.position = transform.position;
        PoolManager.Instance.Push(this);
    }
}
