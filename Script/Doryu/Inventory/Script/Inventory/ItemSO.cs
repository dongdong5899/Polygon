using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EItemName
{
    Stone = 0,
    Wood,
    Copper,
    Iron,
    Ruby,
    Branch,
    Leaf,
    Soil,




    WoodSword = 1000,
    WoodAxe,

}

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemSprite;
    public EItemName itemEnum;
    public SkillSO skillSO;
    //public int maxOverlapAmount = 1;

    //public SerializedDictionary<ItemSO, int> itemRecipe;
}