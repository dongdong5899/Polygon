using ObjectPooling;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [field:SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    public void Init()
    {
        Debug.Log("»ý¼º");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    PoolManager.Instance.Push(this);
        //}
    }
}
