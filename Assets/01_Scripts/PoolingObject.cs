using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public bool onUse;
    public long id;
    public PoolingPackage poolingPackage;

    public void Init(long _id, PoolingPackage _poolingPackage)
    {
        id = _id;
        poolingPackage = _poolingPackage;
        InitListener();
    }

    public void On()
    {
        gameObject.SetActive(true);
        OnListener();
        onUse = true;
    }
    public void Off()
    {
        onUse = false;
        OffListener();
        gameObject.SetActive(false);
    }

    protected virtual void InitListener() { }
    protected virtual void OnListener() { }
    protected virtual void OffListener() { }
}
