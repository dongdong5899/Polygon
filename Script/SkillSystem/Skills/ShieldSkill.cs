using Doryu.StatSystem;
using System;
using UnityEngine;
using YH.Entities;
using YH.Players;

public class ShieldSkill : Skill
{
    private readonly static int[] _HealthByLevel = new int[3] { 2000, 3000, 6000 };

    private Player _player;
    private StatElement _damageStat;
    private StatElement _sizeStat;
    private EntityRenderer _renderer;

    public Shield shield;
    private float _shieldSpawnDelay = 10;
    private float _lastBreakTime = -100;

    public override void Init(SkillSO skillSO, Entity owner)
    {
        base.Init(skillSO, owner);
        _player = PlayerManager.Instance.Player;
        _damageStat = _player.GetCompo<StatCompo>().GetElement("Damage");
        _sizeStat = _player.GetCompo<StatCompo>().GetElement("Size");
        _renderer = _player.GetCompo<EntityRenderer>(true);

        shield = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Shield) as Shield;
        shield.HealthCompo.OnDieEvent += OnShieldBreakEvent;
        shield.gameObject.SetActive(false);

        _lastBreakTime = -100;
    }

    public override Skill Clone()
    {
        ShieldSkill skill = new ShieldSkill();
        return skill;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        shield.HealthCompo.MaxHealthElement.AddModify("ShieldHealth", _HealthByLevel[Level] - 2000);
        shield.HealthCompo.Resurrection();
        shield.SetLevel(Level);
    }

    public override string GetSkillDescriptionByLevel()
    {
        string description = skillSO.itemDescription;
        string key = "<health>";
        int argIndex = description.IndexOf(key);
        description = description.Remove(argIndex, key.Length);
        description = description.Insert(argIndex, $"{_HealthByLevel[Level]}");

        return description;
    }

    public override void OnUseSkill()
    {
        if (shield == null || shield.gameObject.activeSelf == false)
        {
            if (_lastBreakTime + _shieldSpawnDelay < Time.time)
            {
                SettingShield();
            }
        }
    }

    private void SettingShield()
    {
        shield.gameObject.SetActive(true);

        SetRotation(PlayerManager.Instance.CurrentPlayerPoint);
    }

    private void OnShieldBreakEvent()
    {
        _lastBreakTime = Time.time;
        shield.gameObject.SetActive(false);
    }

    private void SetRotation(int index)
    {
        float rotationDeg = 360f / index * _edge + 90;
        float rotation = rotationDeg * Mathf.Deg2Rad;

        shield?.SetPosition(_owner.GetComponent<Entity>().GetCompo<EntityRenderer>().transform.parent,
            _sizeStat.Value, new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)), rotationDeg);
    }

    public override void OnEquipSkill()
    {
        base.OnEquipSkill();
        PlayerManager.Instance.OnChangedPlayerPointEvent += SetRotation;
    }

    public override void OnUnEquipSkill()
    {
        base.OnUnEquipSkill();
        PlayerManager.Instance.OnChangedPlayerPointEvent -= SetRotation;
        if (shield != null)
            shield.gameObject.SetActive(false);
    }
}
