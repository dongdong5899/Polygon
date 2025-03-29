using UnityEngine;

public class Item
{
    public SkillSO SkillSO { get; private set; }
    public Skill Skill { get; private set; }
    //public List<Module> assingedModule;

    public Item(SkillSO skillSO)
    {
        SkillSO = skillSO;
        Skill = skillSO.GetSkill(PlayerManager.Instance.Player);
    }
    public Item(SkillSO skillSO, Skill skill)
    {
        SkillSO = skillSO;
        Skill = skill;
    }

    public bool IsSameItem(Item targetItem)
        => SkillSO.skillName == targetItem.SkillSO.skillName;

    public Item Clone()
    {
        Item item = new Item(SkillSO, Skill);
        return item;
    }
}
