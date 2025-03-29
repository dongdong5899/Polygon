using Doryu.StatSystem;
using System;
using UnityEngine;
using YH.Entities;

public abstract class Skill
{
    public SkillSO skillSO { get; private set; }
    protected Entity _owner; //나중에 Entity 같은거 쓰게 되면 그걸로 바꾸기
    protected StatElement _attackSpeed;

    protected float _prevSkillTime;
    protected float _skillCoolTime;
    protected bool _useCoolTime;
    protected bool _isPassiveSkill;
    protected int _edge;
    protected event Action<int> _OnEdgeChanged;

    public bool _isCircleSkillMode;

    public int Level { get; private set; } = 0;

    public void SetEdge(int edge)
    {
        _edge = edge;
        _OnEdgeChanged?.Invoke(edge);
    }

    public virtual void LevelUp()
    {
        Level++;
    }

    public virtual void Init(SkillSO skillSO, Entity owner)
    {
        _owner = owner;
        this.skillSO = skillSO;

        _skillCoolTime = skillSO.coolTime;
        _useCoolTime = skillSO.useCoolTime;
        _isPassiveSkill = skillSO.skillType == SkillType.PassiveSkill;
        _attackSpeed = PlayerManager.Instance.Player.GetCompo<StatCompo>().GetElement("AttackSpeed");
    }

    public virtual void OnUpdate()
    {
        if (_isPassiveSkill == false) return;

        if (_useCoolTime == false)
        {
            OnUseSkill();
            return;
        }

        if (_prevSkillTime + _skillCoolTime / _attackSpeed.Value < Time.time)
        {
            _prevSkillTime = Time.time;
            OnUseSkill();
        }
    }

    public abstract void OnUseSkill();

    public virtual string GetSkillDescriptionByLevel()
    {
        return skillSO.itemDescription;
    }

    public virtual void OnEquipSkill()
    {
        _prevSkillTime = Time.time;
    }

    public virtual void OnUnEquipSkill()
    {

    }

    public abstract Skill Clone();
}
