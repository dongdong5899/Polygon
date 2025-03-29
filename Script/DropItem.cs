using ObjectPooling;
using UnityEngine;
using YH.Players;

public class DropItem : MonoBehaviour, IPoolable
{
    [SerializeField] private SkillSO _skillSO;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _whatIsPlayer;

    private CircleCaster2D _circleCaster;
    
    public GameObject GameObject { get => gameObject; set { } }
    [field:SerializeField] public PoolingType PoolType { get; set; }

    private void Awake()
    {
        _circleCaster = GetComponent<CircleCaster2D>();
    }

    private void Update()
    {
        if (_circleCaster.CheckCollision(out RaycastHit2D[] hits, _whatIsPlayer))
        {
            if (hits[0].transform.TryGetComponent(out Player player))
            {
                InventoryManager.Instance.AddItem(EInventory.Main, _skillSO);
                PoolManager.Instance.Push(this);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Init()
    {

    }

    public void Setting(SkillSO skillSO, Vector3 pos)
    {
        transform.position = pos;
        _skillSO = skillSO;
        _spriteRenderer.sprite = _skillSO.icon;
    }
}
