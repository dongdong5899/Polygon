using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(menuName = "SO/Pool/PoolItem")]
    public class PoolingItemSO : ScriptableObject
    {
        public int poolCount;
        public MonoBehaviour prefab;
        public IPoolable PoolObj => prefab as IPoolable;
    }

}

