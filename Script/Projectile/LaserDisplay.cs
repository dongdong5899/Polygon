using DG.Tweening;
using ObjectPooling;
using UnityEngine;

public class LaserDisplay : MonoBehaviour, IPoolable
{
    private Transform _parentTrm;
    private bool _isEnable;
    [SerializeField] private Transform _fill;
    [SerializeField] private float _lifetime = 1.5f;
    private float _popTime;
    private bool _ficking;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }


    public void Init()
    {
        _isEnable = true;
        _ficking = false;
        _popTime = Time.time;
        _fill.DOScaleX(0, 0).SetAutoKill(true);
        _fill.DOScaleX(1, _lifetime).SetAutoKill(true);
    }

    public void Setting(Transform parentTrm)
    {
        _parentTrm = parentTrm;
    }
    public void FixAim()
    {
        _ficking = true;
    }
    private void Update()
    {
        if (_lifetime + _popTime < Time.time|| !_parentTrm.gameObject.activeInHierarchy)
        {
            _isEnable = false;
            PoolManager.Instance.Push(this);
        }

        if (_ficking) return;
        transform.rotation = _parentTrm.rotation;
    }
}
