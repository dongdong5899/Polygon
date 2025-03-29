using YH.Entities;

public class DamageIncreaseSkill : StatUpSkill
{
    public override void Init(SkillSO skillSO, Entity owner)
    {
        AddModifyData("Damage", 10f, true);
        base.Init(skillSO, owner);
    }

    public override Skill Clone()
    {
        Skill skill = new DamageIncreaseSkill();
        return skill;
    }
}
