using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<PoolingType, Pool<IPoolable>> _poolDictionary
        = new Dictionary<PoolingType, Pool<IPoolable>>();

    public PoolList poolListSO;

    private void Awake()
    {
        foreach(PoolingItemSO item in poolListSO.GetList())
        {
            CreatePool(item);
        }
    }

    private void CreatePool(PoolingItemSO item)
    {
        Pool<IPoolable> pool = new Pool<IPoolable>(
            item.PoolObj, item.PoolObj.PoolType, transform, item.poolCount);
        _poolDictionary.Add(item.PoolObj.PoolType, pool);
    }

    public IPoolable Pop(PoolingType type)
    {
        if(_poolDictionary.ContainsKey(type) == false)
        {
            Debug.LogError($"Prefab does not exist on pool : {type.ToString()}");
            return null;
        }

        IPoolable item = _poolDictionary[type].Pop();
        item.Init();
        return item;
    }

    public void Push(IPoolable obj, bool resetParent = false)
    {
        if (resetParent)
            obj.GameObject.transform.parent = transform;
        _poolDictionary[obj.PoolType].Push(obj);
    }

}
