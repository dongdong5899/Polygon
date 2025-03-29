using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DashSkillUI : MonoBehaviour
{
    [SerializeField] private RectTransform _leftMask, _rightMask;
    [SerializeField] private Image _left, _right;

    private bool _isActive = false;

    public void SetDashUIAmount(float amount)
    {
        float myAmount = 1 - amount;
        _leftMask.sizeDelta = new Vector2(myAmount * 200f, 55f);
        _rightMask.sizeDelta = new Vector2(myAmount * 200f, 55f);

        if (_isActive && myAmount < 1f) _isActive = false;
        if (_isActive == false && myAmount >= 1f)
        {
            _isActive = true;
            Color color = _left.color;
            _left.color = Color.white;
            _right.color = Color.white;
            _left.DOKill();
            _left.DOColor(color, 0.3f);
            _right.DOKill();
            _right.DOColor(color, 0.3f);
        }
    }
}
