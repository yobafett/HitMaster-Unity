using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BulletPool 
{
    public bool autoExpand { get; set; }

    private GameObject _prefab;
    private Transform _container;
    private List<BulletMove> _pool;

    public BulletPool(GameObject prefab, int count)
    {
        _prefab = prefab;
        _container = null;
        
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<BulletMove>();
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private BulletMove CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        var bulletMove = createdObject.GetComponent<BulletMove>();
        _pool.Add(bulletMove);
        return bulletMove;
    }

    private bool HasFreeElement(out BulletMove element)
    {
        var count = _pool.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_pool[i].gameObject.activeInHierarchy)
            {
                element = _pool[i];
                _pool[i].gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public BulletMove GetFreeElement()
    {
        if (HasFreeElement(out var element))
        {
            return element;
        }

        if (autoExpand)
        {
            return CreateObject(true);
        }

        throw new Exception($"There is no free elements in pool type {typeof(GameObject)}");
    }
}