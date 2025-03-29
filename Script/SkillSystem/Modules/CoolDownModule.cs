using UnityEngine;

public class CoolDownModule : Module
{
    private float _coolDownValue = 0.5f;

    public override void EquipModule(Skill skill)
    {
        skill.skillSO.coolTime -= _coolDownValue;
    }

    public override void UnEquipModule(Skill skill)
    {
        skill.skillSO.coolTime += _coolDownValue;
    }

    public override Module Clone()
    {
        CoolDownModule module = new CoolDownModule();
        module._coolDownValue = _coolDownValue;
        return module;
    }
}
