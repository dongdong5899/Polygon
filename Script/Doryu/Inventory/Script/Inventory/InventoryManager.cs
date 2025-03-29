using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EInventory
{
    Main,
    Equip
}

public class InventoryManager : MonoSingleton<InventoryManager>
{
    [SerializeField] private SkillSO _itemSO1;
    [SerializeField] private SkillSO _itemSO2;
    private Dictionary<EInventory, Inventory> _inventoryDictionary;
    private Slot _dragSlot;
    public Slot DragSlot
    {
        get => _dragSlot;
        set => _dragSlot = value;
    }
    private Slot _pointerSlot;
    public Slot PointerSlot
    {
        get => _pointerSlot;
        set => _pointerSlot = value;
    }

    public Action<int, Item> OnQuickSlotChangeEvent;

    private void Awake()
    {
        _inventoryDictionary = new Dictionary<EInventory, Inventory>();
        FindObjectsByType<Inventory>(FindObjectsSortMode.None).ToList()
            .ForEach(inventory =>
            {
                inventory.Init();
                _inventoryDictionary.Add(inventory.inventoryEnum, inventory);
            });
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            AddItem(EInventory.Main, _itemSO1);
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            AddItem(EInventory.Main, _itemSO2);
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Item skill = new Item();
        //    skill.skill = _itemSO1;
        //    skill.SetAmount(1);
        //    AddItem(EInventory.Main, skill);
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    Item skill = new Item();
        //    skill.skill = _itemSO2;
        //    skill.SetAmount(1);
        //    AddItem(EInventory.Main, skill);
        //}

        //for (int i = 0; i < 9; i++)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
        //    {
        //        OnQuickSlotChangeEvent?.Invoke(i, GetSlot(EInventory.Quick, i).GetAssignedItem());
        //    }
        //}
    }

    public Slot GetSlot(EInventory inventory, int index)
        => _inventoryDictionary[inventory].GetSlot(index);
    public Slot GetSlot(EInventory inventory, Vector2Int pos)
        => _inventoryDictionary[inventory].GetSlot(pos);

    public bool AddItem(EInventory inventory, Item item)
    {
        return _inventoryDictionary[inventory].AddItem(item);
    }
    public bool AddItem(EInventory inventory, SkillSO itemSO, int amount = 1)
    {
        Item item = new Item(itemSO);
        return _inventoryDictionary[inventory].AddItem(item);
    }
    public void RemoveItem(EInventory inventory, Item item)
    {
        _inventoryDictionary[inventory].AddItem(item);
    }
    public void RemoveItem(EInventory inventory, SkillSO itemSO, int amount = 1)
    {
        Item item = new Item(itemSO);
        //skill.SetAmount(amount);
        _inventoryDictionary[inventory].RemoveItem(item);
    }
}
