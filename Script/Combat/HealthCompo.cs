using DG.Tweening;
using Doryu.StatSystem;
using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;
using YH.Entities;
using YH.Players;
using Random = UnityEngine.Random;

public class HealthCompo : MonoBehaviour, IEntityComponent, IAfterInitable
{
    public int Health { get; private set; }

    public float LastAttackTime { get; set; }
    [SerializeField] private StatElementSO _healthSO;

    private Entity _owner;
    public StatElement MaxHealthElement { get; private set; }
    private bool _isInvincible;
    private bool _isDie;

    public int MaxHealth => Mathf.CeilToInt(MaxHealthElement.Value);
    public event Action<int, int, bool> OnHealthChangedEvent;
    public event Action OnDieEvent;

    public void Initialize(Entity entity)
    {
        _owner = entity;
    }

    public void SetInvincible(bool isInvinvible)
    {
        _isInvincible = isInvinvible;
    }

    public void AfterInit()
    {
        MaxHealthElement = _owner.GetCompo<StatCompo>().GetElement(_healthSO);
        _isInvincible = MaxHealthElement == null;
        Health = MaxHealth;

        if (MaxHealthElement != null) MaxHealthElement.OnValueChangedEvent += HandleMaxHealthChangedEvent;
    }

    private void HandleMaxHealthChangedEvent(float prev, float current)
    {
        int prevHealth = Health;
        if (Health > current) Health = Mathf.CeilToInt(current);
        OnHealthChangedEvent?.Invoke(prevHealth, Health, false);
    }

    public void ApplyDamage(StatCompo statCompo, int damage, bool isChangeVisible = true, bool isTextVisible = true)
    {
        if (_isDie) return;
        if (_isInvincible) return;


        bool isCritical = false;
        float random = Random.Range(0f, 100f);
        if (random < statCompo.GetElement("Critical").Value)
        {
            isCritical = true;
            damage = Mathf.CeilToInt(damage * (statCompo.GetElement("CriticalDamage").Value / 100));
        }

        int prev = Health;
        Health -= damage;
        AudioManager.Instance.PlaySound(EAudioName.Hit);
        if (Health < 0)
            Health = 0;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);

        if (_owner is not Player)
        {
            PlayerManager.Instance.AddCircleSkillGauge(damage * (1f / 1000));
        }

        if (isTextVisible)
        {
            DamageText damageText = PoolManager.Instance.Pop(PoolingType.DamageText) as DamageText;
            damageText.Setting(damage, isCritical, transform.position);
        }

        if (Health == 0) Die();
    }

    

    public void ApplyRecovery(int recovery, bool isChangeVisible = true)
    {
        if (_isDie) return;

        int prev = Health;
        Health += recovery;
        if (Health > MaxHealth)
            Health = MaxHealth;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);
    }

    public void Resurrection()
    {
        _isDie = false;
        ApplyRecovery(MaxHealth, false);
    }

    public void Die()
    {
        _isDie = true;
        OnDieEvent?.Invoke();
        if (MaxHealthElement != null) MaxHealthElement.OnValueChangedEvent -= HandleMaxHealthChangedEvent;
    }
}
