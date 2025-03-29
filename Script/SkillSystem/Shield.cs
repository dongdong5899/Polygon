using ObjectPooling;
using System;
using UnityEngine;
using YH.Entities;

public class Shield : Entity, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color[] _colorByLevel;

    public HealthCompo HealthCompo { get; private set; }

    public GameObject GameObject { get => gameObject; set { } }
    [field: SerializeField] public PoolingType PoolType { get; set; }

    public bool IsDead { get; private set; }

    public void Init()
    {
        SetLevel(0);
    }

    public void SetLevel(int level)
    {
        _spriteRenderer.color = _colorByLevel[level];
    }

    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();

        HealthCompo = GetCompo<HealthCompo>();
        HealthCompo.OnHealthChangedEvent += HandleHealthChanged;
        HealthCompo.OnDieEvent += Die;
    }

    private void HandleHealthChanged(int prev, int current, bool isChangeVisible)
    {
        Color color = _spriteRenderer.color;
        color.a = (float)current / HealthCompo.MaxHealth + 0.5f;
        _spriteRenderer.color = color;
    }

    public void SetPosition(Transform parent, float radius, Vector3 dir, float angle)
    {
        transform.SetParent(parent);
        transform.localPosition = dir * radius * 1.5f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void Die()
    {
        HealthCompo.Resurrection();
    }
}
