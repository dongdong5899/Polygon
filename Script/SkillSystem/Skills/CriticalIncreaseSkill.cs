using UnityEngine;
using YH.Entities;

public class CriticalIncreaseSkill : StatUpSkill
{
    public override void Init(SkillSO skillSO, Entity owner)
    {
        AddModifyData("Critical", 10f, false);
        AddModifyData("CriticalDamage", 50f, false, "CriticalDamage");
        base.Init(skillSO, owner);
    }

    public override Skill Clone()
    {
        Skill skill = new CriticalIncreaseSkill();
        return skill;
    }
}
