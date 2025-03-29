using UnityEngine;

public class TestSkill : Skill
{
    public override void OnUnEquipSkill()
    {
        Debug.Log("TEST스킬 장착 해제");
    }

    public override void OnEquipSkill()
    {
        Debug.Log("TEST스킬 장착");
    }

    public override void OnUseSkill()
    {
        Debug.Log("TEST");
    }

    public override Skill Clone()
    {
        Skill skill = new TestSkill();
        return skill;
    }
}
