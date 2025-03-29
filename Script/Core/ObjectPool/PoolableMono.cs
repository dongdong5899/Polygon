using UnityEngine;

namespace ObjectPooling
{
    public interface IPoolable
    {
        public GameObject GameObject { get; set; }
        public PoolingType PoolType { get; set; }
        public void Init();
    }
}
