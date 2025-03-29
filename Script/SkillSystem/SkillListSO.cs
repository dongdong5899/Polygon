using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSetSO", menuName = "SO/SkillSet")]
public class SkillListSO : ScriptableObject
{
    public List<SkillSO> skillList;
}
