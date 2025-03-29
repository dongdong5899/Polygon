using ObjectPooling;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AreaAttack : MonoBehaviour, IPoolable
{
    [SerializeField] private RemainEffect _remainEffect;
    private Vector2 _targetPosition;
    private float _speed;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    private void Update()
    {
        Vector2 dir = (_targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)dir * _speed * Time.deltaTime;

        if (Vector2.Distance(_targetPosition, (Vector2)transform.position) <= 0.3f)
        {
            Instantiate(_remainEffect, transform.position, Quaternion.identity);
            PoolManager.Instance.Push(this);
        }
    }

    public void Init() {}

    public void Setting(Vector2 target, float speed)
    {
        _targetPosition = target;
        _speed = speed;
    }
}
