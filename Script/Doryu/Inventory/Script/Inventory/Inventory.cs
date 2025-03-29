using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public EInventory inventoryEnum;
    [field:SerializeField] public SkillInfoUI skillInfoUI { get; private set; }
    [field: SerializeField] public TextBoxUI textBox { get; private set; }

    [SerializeField] protected Transform _inventoryTrm;
    [SerializeField] protected Vector2Int _inventorySize;
    protected List<Slot> _slots;

    public Transform dragItemTrm;

    public virtual void Init()
    {
        _slots = new List<Slot>();
        for (int i = 0; i < _inventorySize.y * _inventorySize.x; i++)
        {
            Slot slot = _inventoryTrm.GetChild(i).GetComponent<Slot>();
            slot.SlotInit(this);

            _slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        bool canInsertItem = false;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].TryGetAssignedItem(out Item slotItem) == false)
            {
                _slots[i].AssignItem(item);
                canInsertItem = true;
                break;
            }
        }

        return canInsertItem;
    }

    public bool RemoveItem(Item item)
    {
        Slot slot = _slots.Find(slot => slot.GetAssignedItem() == item);
        if (slot != null)
            slot.AssignItem(null);

        return slot != null;
    }

    public Slot GetSlot(int index)
    {
        if (_slots.Count > index)
        {
            return _slots[index];
        }
        return null;
    }

    public Slot GetSlot(Vector2Int pos)
    {
        if (_slots.Count / _inventorySize.x > pos.y &&
            _slots.Count / _inventorySize.y > pos.x)
        {
            return _slots[pos.x + pos.y * _inventorySize.x];
        }
        return null;
    }
}