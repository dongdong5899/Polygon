using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private readonly static Color[] _Colors = new Color[3] { Color.white, Color.yellow, Color.red };

    [SerializeField] protected RectTransform _itemRectTrm;
    [SerializeField] protected Image _selectOutline;
    [SerializeField] protected TextMeshProUGUI _levelText;

    protected Image _itemImage;
    protected Inventory _inventory;
    protected Item _assignedItem;
    protected Vector3 _dragStartPos;
    protected bool _isDrag;
    protected int _idx = 0;

    public event Action<Item, int> OnAddItem;
    public event Action<Item, int> OnRemoveItem;
    public RectTransform RectTrm => transform as RectTransform;

    public virtual void SlotInit(Inventory inventory)
    {
        _inventory = inventory;
        _selectOutline.color = new Color(1, 1, 1, 0);
        _itemImage = _itemRectTrm.GetComponent<Image>();
        AssignItem(null);
    }


    public virtual void AssignItem(Item item)
    {
        if (item == null) OnRemoveItem?.Invoke(_assignedItem, _idx);
        else OnAddItem?.Invoke(item, _idx);

        _assignedItem = item;

        //TextUpdate();
        ImageUpdate();
    }

    //public void TextUpdate()
    //{
    //    _levelText.text = _assignedItem == null ? 
    //        string.Empty : RomeNumber.GetNumber(_assignedItem.Skill.level + 1);
    //}
    public void ImageUpdate()
    {
        if (_assignedItem == null)
            _itemImage.color = Color.clear;
        else
        {
            _itemImage.color = _Colors[_assignedItem.Skill.Level];
            _itemImage.sprite = _assignedItem.SkillSO.icon;
        }
    }

    public void ChangeSlot(Slot slot)
    {
        Item item = slot.GetAssignedItem()?.Clone();
        slot.AssignItem(null);
        slot.AssignItem(_assignedItem);
        AssignItem(null);
        AssignItem(item);
    }


    public bool TryGetAssignedItem(out Item item)
    {
        item = _assignedItem;
        return _assignedItem != null;
    }
    public Item GetAssignedItem()
    {
        return _assignedItem;
    }

    public void SetSlotIdx(int idx)
        => _idx = idx;

    public void SetPosition(Vector2 position)
        => RectTrm.anchoredPosition = position;

    #region InputEvents

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDrag) return;
        _selectOutline.color = new Color(1, 1, 0, 0.8f);
        InventoryManager.Instance.PointerSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDrag) return;
        _selectOutline.color = Color.clear;
        InventoryManager.Instance.PointerSlot = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_assignedItem == null) return;

        _isDrag = true;
        _selectOutline.color = Color.clear;
        _dragStartPos = Input.mousePosition * (1920f / Screen.width);
        InventoryManager.Instance.DragSlot = this;
        _itemRectTrm.SetParent(_inventory.dragItemTrm);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDrag)
        {
            InventoryManager.Instance.DragSlot = null;
            Slot pointerSlot = InventoryManager.Instance.PointerSlot;
            if (pointerSlot != null && pointerSlot != this)
            {
                Item pointerItem = pointerSlot.GetAssignedItem();
                if (_assignedItem != null && pointerItem != null)
                {
                    bool isSameItem = _assignedItem.IsSameItem(pointerItem);

                    if (isSameItem && pointerItem.Skill.Level == _assignedItem.Skill.Level && pointerItem.Skill.Level < 2)
                    {
                        pointerItem.Skill.LevelUp();
                        pointerSlot.ImageUpdate();
                        AssignItem(null);
                    }
                    else
                    {
                        pointerSlot.ChangeSlot(this);
                    }
                }
                else
                {
                    pointerSlot.ChangeSlot(this);
                }
            }
            _itemRectTrm.SetParent(transform);
        }

        _itemRectTrm.anchoredPosition = Vector3.zero;
        _isDrag = false;
        //디버그
        if (_assignedItem != null)
            Debug.Log(_assignedItem.Skill.Level);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDrag == false) return;

        Vector2 mousePosition = Input.mousePosition;
        float screenRatio = 1920f / Screen.width;

        _itemRectTrm.anchoredPosition = mousePosition * screenRatio;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
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

            if (InventoryManager.Instance.AddItem(EInventory.Equip, item) == false)
            {
                _inventory.textBox.SetText("아이템을 못 넣어요ㅠㅠ");
                _inventory.textBox.Open();
                return;
            }
            AssignItem(null);
        }
    }

    #endregion
}
