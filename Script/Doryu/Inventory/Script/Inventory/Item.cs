using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public SkillSO skill;
    public int level = 0;
    public List<Module> modules = new List<Module>();
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

    public void AddModule(Module module)
    {
        modules.Add(module);
    }

    public Item Clone()
    {
        Item item = new Item(skill);
        item.level = this.level;
        return item;
    }
}
