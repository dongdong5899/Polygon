using UnityEngine;
using YH.Entities;

public class SpeedIncreaseSkill : StatUpSkill
{
    public override void Init(SkillSO skillSO, Entity owner)
    {
        AddModifyData("Speed", 10f, true);
        base.Init(skillSO, owner);
    }

    public override Skill Clone()
    {
        Skill skill = new SpeedIncreaseSkill();
        return skill;
    }
}
