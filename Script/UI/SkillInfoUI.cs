using DG.Tweening;
using TMPro;
using UnityEngine;

public class SkillInfoUI : MonoBehaviour, IWindowPanel
{
    private Tween _tween;
    private SkillSO _skillSO;
    private Slot _slot;

    [SerializeField] private TextMeshProUGUI _nameTxt;
    [SerializeField] private TextMeshProUGUI _explainTxt;
    [SerializeField] private TextMeshProUGUI _levelTxt;
    [SerializeField] private TextBoxUI _textBox;

    private RectTransform _rectTrm => transform as RectTransform;

    private void Awake()
    {
        _rectTrm.localScale = Vector3.zero;
    }

    public void SetSkillInfo(Slot slot)
    {
        Item item = slot.GetAssignedItem();
        _slot = slot;

        if (item == null)
        {
            _skillSO = null;
            _nameTxt.SetText(string.Empty);
            _explainTxt.SetText(string.Empty);
            _levelTxt.SetText("Lv.");
        }
        else
        {
            _skillSO = item.SkillSO;
            _nameTxt.SetText($"{_skillSO.skillName}Skill");
            _explainTxt.SetText(item.Skill.GetSkillDescriptionByLevel());
            _levelTxt.SetText($"Lv.{item.Skill.Level + 1}");
        }

    }

    public void InsertItem()
    {
        if (_skillSO == null) return;

        if(InventoryManager.Instance.AddItem(EInventory.Equip, _skillSO) == false)
        {
            _textBox.SetText("아이템을 넣을 수 없습니다.");
            _textBox.Open();
            return;
        }
        _slot.AssignItem(null);
        Close();
    }

    public void SetPosition(Vector2 position)
        => _rectTrm.anchoredPosition = position;

    public void Open()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOScale(1, 0.2f).SetUpdate(true);
    }

    public void Close()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();

        _tween = _rectTrm.DOScale(0, 0.2f).SetUpdate(true);
    }
}
