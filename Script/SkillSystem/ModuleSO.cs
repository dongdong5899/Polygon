using UnityEngine;

[CreateAssetMenu(fileName = "ModuleSO", menuName = "SO/Module")]
public class ModuleSO : ScriptableObject
{
    public string moduleName;
    public Sprite icon;
    [TextArea]
    public string moduleDescription;
}
