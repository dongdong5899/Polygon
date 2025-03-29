using System;
using UnityEngine;
using YH.Entities;

public enum SkillType
{
    PassiveSkill,
    ActiveSkill,
    StatUpSkill
}


[CreateAssetMenu(fileName = "SkillSO", menuName = "SO/Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    [TextArea]
    public string itemDescription;

    [Space(10)]
    public SkillType skillType;
    public bool useCoolTime;
    public float coolTime;

    private Skill _skill;

    private void OnEnable()
    {
        try
        {
            Type t = Type.GetType($"{skillName}Skill");
            _skill = Activator.CreateInstance(t) as Skill;
        }
        catch (Exception e)
        {
            Debug.LogError($"Skill name of {skillName} is not exsist");
            Debug.LogException(e);
        }
    }

    public Skill GetSkill(Entity owner)
    {
        Skill skill = _skill.Clone();
        skill.Init(this, owner);
        return skill;
    }
}
