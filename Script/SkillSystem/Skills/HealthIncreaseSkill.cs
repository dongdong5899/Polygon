using UnityEngine;
using YH.Entities;

public class HealthIncreaseSkill : StatUpSkill
{
    public override void Init(SkillSO skillSO, Entity owner)
    {
        AddModifyData("Health", 30f, true);
        AddModifyData("Size", 5f, true);
        base.Init(skillSO, owner);
    }

    public override Skill Clone()
    {
        Skill skill = new HealthIncreaseSkill();
        return skill;
    }
}
