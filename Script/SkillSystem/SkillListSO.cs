using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSetSO", menuName = "SO/SkillSet")]
public class SkillListSO : ScriptableObject
{
    public List<SkillSO> skillList;
}
