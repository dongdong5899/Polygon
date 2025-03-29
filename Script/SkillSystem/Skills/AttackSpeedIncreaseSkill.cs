using UnityEngine;
using YH.Entities;

public class AttackSpeedIncreaseSkill : StatUpSkill
{
    public override void Init(SkillSO skillSO, Entity owner)
    {
        AddModifyData("AttackSpeed", 10f, true);
        base.Init(skillSO, owner);
    }

    public override Skill Clone()
    {
        Skill skill = new AttackSpeedIncreaseSkill();
        return skill;
    }
}
