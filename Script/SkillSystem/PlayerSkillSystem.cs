using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using YH.Entities;
using YH.Players;

public class PlayerSkillSystem : MonoBehaviour, IEntityComponent, IAfterInitable
{
    private Player _player;

    private List<Skill> _enabledSkillList = new List<Skill>(3);

    public void Initialize(Entity entity)
    {
        _player = entity as Player;
    }

    public void AfterInit()
    {
        _enabledSkillList = new List<Skill> { null, null, null };
    }

    private void Update()
    {
        _enabledSkillList.ForEach(skill => skill?.OnUpdate());
    }

    public void SetSlot(List<Skill> skills)
    {
        int currentPointCnt = PlayerManager.Instance.CurrentPlayerPoint;
        List<Skill> prevSkill = _enabledSkillList.ToList();
        _enabledSkillList.Clear();

        for (int i = 0; i < skills.Count; i++)
        {
            Skill skill = skills[i];

            if (skill == null)
            {
                _enabledSkillList.Add(null);
            }
            else
            {
                if (prevSkill.Count > i && prevSkill[i] != null
                    && prevSkill[i] == skill)
                {
                    _enabledSkillList.Add(skill);
                }
                else
                {
                    skill.SetEdge(i % currentPointCnt);
                    skill.OnEquipSkill();
                    _enabledSkillList.Add(skill);
                }
            }
        }
    }

    public void EquipSkill(Skill skill, int idx)
    {
        int currentPointCnt = PlayerManager.Instance.CurrentPlayerPoint;

        if (_enabledSkillList[idx] == null)
        {
            _enabledSkillList[idx] = skill;
            skill.SetEdge(idx % currentPointCnt);
            skill.OnEquipSkill();
        }
    }

    public void UnEquipSkill(Skill skill, int idx)
    {
        if (_enabledSkillList[idx] != null)
        {
            _enabledSkillList[idx].OnUnEquipSkill();
            _enabledSkillList[idx] = null;
        }
    }
}
