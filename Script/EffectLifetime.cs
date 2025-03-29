using ObjectPooling;
using UnityEngine;

public class EffectLifetime : MonoBehaviour, IPoolable
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private bool _isPooling;
    private float _popTime;
    public GameObject GameObject { get => gameObject; set { } }
    [field:SerializeField] public PoolingType PoolType { get; set; }

    private void Start()
    {
        if (_isPooling == false) Init();
    }

    public void Init()
    {
        _popTime = Time.time;
    }

    private void Update()
    {
        if (_lifeTime + _popTime < Time.time)
        {
            if (_isPooling)
                PoolManager.Instance.Push(this);
            else
                Destroy(gameObject);
        }
    }
}
