using Doryu.StatSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YH.Entities;
using YH.Players;

public struct StatModifyData
{
    public float increaseValue;
    public string modifierKey;
    public StatElement statElement;
    public bool useByPercentage;
}

public abstract class StatUpSkill : Skill
{
    private StatElement _statElement;

    protected List<StatModifyData> _statModifyDatasList = new List<StatModifyData>();

    public override void Init(SkillSO skillSO, Entity owner)
    {
        base.Init(skillSO, owner);
    }

    public void AddModifyData(string elementKey, float increaseValue, bool useByPercentage, string GUID = "")
    {
        StatModifyData statModifyData = new StatModifyData();
        if (GUID == "")
        {
            Guid guid = Guid.NewGuid();
            statModifyData.modifierKey = guid.ToString();
        }
        else
            statModifyData.modifierKey = GUID;

        statModifyData.statElement = PlayerManager.Instance.Player.GetCompo<StatCompo>().GetElement(elementKey);
        statModifyData.increaseValue = increaseValue;
        statModifyData.useByPercentage = useByPercentage;

        _statModifyDatasList.Add(statModifyData);
    }

    public override void OnEquipSkill()
    {
        _statModifyDatasList.ForEach(statModifyData =>
        {
            if (statModifyData.useByPercentage)
                statModifyData.statElement.AddModifyPercent(statModifyData.modifierKey, statModifyData.increaseValue);
            else
                statModifyData.statElement.AddModify(statModifyData.modifierKey, statModifyData.increaseValue);
        });
    }

    public override void OnUnEquipSkill()
    {
        _statModifyDatasList.ForEach(statModifyData =>
        {
            if (statModifyData.useByPercentage)
                statModifyData.statElement.RemoveModifyPercent(statModifyData.modifierKey);
            else
                statModifyData.statElement.RemoveModify(statModifyData.modifierKey);
        });
    }

    public override void OnUseSkill()
    {

    }
}
