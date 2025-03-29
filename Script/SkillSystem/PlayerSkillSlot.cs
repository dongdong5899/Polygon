using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillSlot : Slot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (_assignedItem == null) return;

        //좌클릭이면 설명 열기
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _inventory.skillInfoUI.SetSkillInfo(this);
            _inventory.skillInfoUI.SetPosition(RectTrm.anchoredPosition);
            _inventory.skillInfoUI.Open();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Item item = _assignedItem.Clone();

            AssignItem(null);
            InventoryManager.Instance.AddItem(EInventory.Main, item);
        }
    }

    public override void AssignItem(Item item)
    {
        base.AssignItem(item);
        if (AudioManager.Instance != null) 
            AudioManager.Instance.PlaySound(EAudioName.EquipItem);  
    }
}
