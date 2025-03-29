using Doryu.StatSystem;
using ObjectPooling;
using System.Collections.Generic;
using UnityEngine;
using YH.Entities;

public class Homing : MonoBehaviour, IPoolable
{
    private Transform _targetTrm;
    private Transform _visualTrm;
    private List<SpriteRenderer> _spriteRendererList;
    private CircleCaster2D _circleCaster;
    private LayerMask _whatIsTarget;
    private float _speed;
    private float _homingValue;
    private int _damage;
    private bool _isEnable;

    [SerializeField] private ParticleSystem _particle;

    [SerializeField] private PoolingType _effectType;
    [SerializeField] private float _lifetime = 3f;
    private float _rotate = 0f;
    private float _popTime;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    private Entity _entity;

    private void Awake()
    {
        _circleCaster = GetComponent<CircleCaster2D>();
        _visualTrm = transform.GetChild(0);
        _spriteRendererList = new List<SpriteRenderer>();
        GetComponentsInChildren(_spriteRendererList);
    }

    private void Start()
    {
        Sprite sprite = PolygonGenerator.GeneratePolygonSprite(7, 0.5f, Color.white);
        _spriteRendererList.ForEach(spriteRenderer => spriteRenderer.sprite = sprite);
        var textureSheetAnimation = _particle.textureSheetAnimation;
        textureSheetAnimation.AddSprite(sprite);
    }

    public void Init()
    {
        _isEnable = true;
        _popTime = Time.time;
        _rotate = 0f;
    }

    public void Setting(Entity entity, LayerMask whatIsTarget, Transform target, float speed, int damage, float homingValue)
    {
        _entity = entity;
        _targetTrm = target;
        _homingValue = homingValue;
        _whatIsTarget = whatIsTarget;
        _speed = speed;
        _damage = damage;
    }

    private void FixedUpdate()
    {
        if (_lifetime + _popTime < Time.time)
        {
            _isEnable = false;
            Die();
        }

        if (_isEnable == false) return;

        _rotate += Time.fixedDeltaTime * 8;
        _visualTrm.Rotate(Vector3.forward * _rotate);

        if (_targetTrm == null)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 50, _whatIsTarget);

            foreach (Collider2D target in targets)
            {
                if (target.TryGetComponent(out Shield shield) == false)
                {
                    _targetTrm = target.transform;
                }
            }
        }
        else
        {
            Vector3 targetDir = _targetTrm.position - transform.position;
            transform.up = (transform.up * _homingValue + targetDir.normalized).normalized;
        }


        Vector3 movement = transform.up * (Time.fixedDeltaTime * _speed);
        if (_circleCaster.CheckCollision(out RaycastHit2D[] hits, _whatIsTarget, movement))
        {
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Entity entity))
                {
                    entity.GetCompo<HealthCompo>().ApplyDamage(_entity.GetCompo<StatCompo>(), _damage);
                    Die();
                    CameraManager.Instance.ShakeCamera(10, 8, 0.15f);
                }
                _isEnable = false;
                transform.position += transform.up * hit.distance;
                break;
            }
        }
        else
        {
            transform.position += movement;
        }
    }

    private void Die()
    {
        Transform effect = PoolManager.Instance.Pop(_effectType).GameObject.transform;
        effect.position = transform.position;
        PoolManager.Instance.Push(this);
    }
}
