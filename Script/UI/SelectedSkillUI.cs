using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectedSkillUI : Inventory
{
    private SkillSystemUI _skillSystemUI;
    [SerializeField] private PlayerSkillSlot _slotPf;
    [SerializeField] private PlayerSkillSystem _skillSystem;
    private List<Vector2> _slotPositions;
    private int _currentLayer = 0;

    private Tween _tween;

    private void Awake()
    {
        _skillSystemUI = GetComponentInParent<SkillSystemUI>();
    }

    public void SetSlotCnt(int cnt, List<Vector2> positions)
    {
        _slotPositions = positions;
        int prevSize = _inventorySize.x;
        _inventorySize.x = cnt;

        //스킬이 더 많을 때, 더 적을 때
        if (prevSize < cnt)
        {
            List<Slot> prevSlotList = _slots.ToList();
            _slots.Clear();

            for (int i = 0; i < _skillSystemUI.LayerCount; i++)
            {
                for (int j = 0; j < prevSize; j++)
                {
                    PlayerSkillSlot slot = prevSlotList[j + prevSize * i] as PlayerSkillSlot;
                    slot.SetSlotIdx(j + cnt * i);

                    _slots.Add(slot);
                }
                for (int j = prevSize; j < cnt; j++)
                {
                    GenerateSlot(j + cnt * i);
                }
            }
        }
        else if (prevSize > cnt)
        {
            List<Slot> prevSlotList = _slots.ToList();
            _slots.Clear();

            for (int i = 0; i < _skillSystemUI.LayerCount; i++)
            {
                for (int j = 0; j < cnt; j++)
                {
                    PlayerSkillSlot slot = prevSlotList[j + prevSize * i] as PlayerSkillSlot;
                    slot.SetSlotIdx(j + cnt * i);

                    _slots.Add(slot);
                }
            }

            for (int i = 0; i < prevSlotList.Count; i++)
            {
                if (_slots.Contains(prevSlotList[i]) == false)
                {
                    if (prevSlotList[i].TryGetAssignedItem(out Item item))
                    {
                        InventoryManager.Instance.AddItem(EInventory.Main, item);
                    }
                    Destroy(prevSlotList[i].gameObject);
                }
            }
        }

        SetSlotPosition(_slotPositions);
        AddSkillToSkillSystem();
    }

    public void AddLayer()
    {
        _inventorySize.y++;

        for (int i = 0; i < _inventorySize.x; i++)
        {
            PlayerSkillSlot skillSlot = Instantiate(_slotPf, _inventoryTrm);
            skillSlot.SlotInit(this);
            skillSlot.SetSlotIdx(i + (_inventorySize.x * (_inventorySize.y - 1)));

            skillSlot.OnAddItem += OnAddItem;
            skillSlot.OnRemoveItem += OnRemoveItem;

            _slots.Add(skillSlot);
        }

        if (_slotPositions != null)
        {
            SetSlotPosition(_slotPositions);
        }

        AddSkillToSkillSystem();
    }

    public void SelectLayer(int layer)
    {
        float diff = _currentLayer - layer;
        _currentLayer = layer;

        if (_slotPositions != null)
        {
            if (_tween != null && _tween.active)
                _tween.Kill();

            SetSlotPosition(_slotPositions);
            _tween = _inventoryTrm.DORotate(new Vector3(0, 0, 360 * diff), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad).SetUpdate(true);
        }
    }

    public void AddSkillToSkillSystem()
    {
        List<Skill> skills = new List<Skill>();

        for (int j = 0; j < _slots.Count; j++)
        {
            PlayerSkillSlot slot = _slots[j] as PlayerSkillSlot;
            skills.Add(slot.GetAssignedItem()?.Skill);
        }

        _skillSystem.SetSlot(skills);
    }

    private void SetSlotPosition(List<Vector2> positions)
    {
        int currentPointCnt = positions.Count;
        for (int i = 0; i < _slots.Count; i++)
        {
            PlayerSkillSlot skillSlot = _slots[i] as PlayerSkillSlot;
            skillSlot.SetPosition(positions[i % currentPointCnt]);

            bool isEnable = (i / currentPointCnt) == _currentLayer;
            skillSlot.gameObject.SetActive(isEnable);
        }
    }

    private void GenerateSlot(int idx)
    {
        PlayerSkillSlot skillSlot = Instantiate(_slotPf, _inventoryTrm);
        skillSlot.SlotInit(this);
        skillSlot.SetSlotIdx(idx);

        skillSlot.OnAddItem += OnAddItem;
        skillSlot.OnRemoveItem += OnRemoveItem;

        _slots.Add(skillSlot);
    }

    #region Events

    private void OnAddItem(Item item, int idx)
    {
        _skillSystem.EquipSkill(item.Skill, idx);
    }

    private void OnRemoveItem(Item item, int idx)
    {
        if (item == null) return;
        _skillSystem.UnEquipSkill(item.Skill, idx);
    }

    #endregion
}
