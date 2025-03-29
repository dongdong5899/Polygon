using ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Pool/PoolList")]
public class PoolList : ScriptableObject
{
    [SerializeField] private List<PoolingItemSO> _list;

    public List<PoolingItemSO> GetList() => _list;
}
