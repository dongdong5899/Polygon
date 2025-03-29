using DG.Tweening;
using ObjectPooling;
using UnityEngine;

public class BombDisplay : MonoBehaviour, IPoolable
{
    [SerializeField] private Transform _fill;
    [SerializeField] private float _lifetime = 1.5f;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    public void Init()
    {
        Sequence seq = DOTween.Sequence().SetAutoKill(true);
        seq.Append(_fill.DOScaleX(0, 0));
        seq.Join(_fill.DOScaleY(0, 0));
        seq.Append(_fill.DOScaleX(1, _lifetime));
        seq.Join(_fill.DOScaleY(1, _lifetime));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public void Setting(float radius,Transform dealer)
    {
        transform.localScale = new Vector3(radius*2, radius*2, 0);
        transform.localPosition = dealer.position;
    }

}
