using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Pooling : Singleton<Manager_Pooling>
{
    public Dictionary<string, PoolingPackage> poolingPackages = new Dictionary<string, PoolingPackage>();

    public long id;

    public Vector3 hidePos; // 카메라에 안보이는 위치
    public void Init()
    {
        hidePos = new Vector3(0, 50000, 0);
    }

    long GetNewID()
    {
        id++;
        return id;
    }

    public PoolingPackage GetPoolingPackage(string key)
    {
        PoolingPackage poolingPackage = null;

        if (!poolingPackages.ContainsKey(key))   // 키가 없는 아이템이면 dict에 새로 추가
        {
            poolingPackage = new PoolingPackage();
            poolingPackages.Add(key, poolingPackage);
        }
        else
        {
            poolingPackage = poolingPackages[key];  // 키가 있는 아이템이면 해당 아이템을 return
        }
        return poolingPackage;
    }

    public PoolingObject GetPoolingObject(string fileName, string pathCreate = "")  // spawmPos, spawnRot이 필요없는 경우에 사용
    {
        return GetPoolingObject(fileName, hidePos, Quaternion.identity, pathCreate);
    }
    public PoolingObject GetPoolingObject(string fileName, Vector3 spawnPos, Quaternion spawnRot, string pathCreate = "")
    {
        PoolingPackage poolingPackage = GetPoolingPackage(fileName);

        PoolingObject poolingObject = poolingPackage.GetPoolingObject();

        if (poolingObject == null)   // 풀링오브젝트가 없을 시 새로 Instantiate
        {
            poolingObject = Instantiate(Resources.Load(pathCreate, typeof(PoolingObject)), spawnPos, spawnRot) as PoolingObject;
            poolingObject.Init(GetNewID(), poolingPackage);
        }

        if (poolingObject != null)
        {
            PoolingObjectInit(poolingObject, spawnPos, spawnRot);
        }

        return poolingObject;
    }

    public void Return(PoolingObject poolingObject)
    {
        poolingObject.poolingPackage.Return(poolingObject);
    }




    public UnitBase GetUnitBase(UnitStats unitStats, Vector3 spawnPos, Quaternion spawnRot)
    {
        string fileName = unitStats.fileName;

        PoolingPackage poolingPackage = GetPoolingPackage(fileName);

        PoolingObject poolingObject = poolingPackage.GetPoolingObject();

        if (poolingObject == null)
        {
            GameObject GO = new GameObject(fileName);

            if (unitStats.unitType == UnitType.Player)
            {
                poolingObject = GO.AddComponent<UnitBase_Player>();
            }
            else if (unitStats.unitType == UnitType.Monster)
            {
                poolingObject = GO.AddComponent<UnitBase_Monster>();
            }

            poolingObject.Init(GetNewID(), poolingPackage);
        }

        if (poolingObject != null)
        {
            PoolingObjectInit(poolingObject, spawnPos, spawnRot);
        }

        return poolingObject.GetComponent<UnitBase>();
    }

    public T GetPoolingObject<T>(string fileName, string pathCreate = "") where T : PoolingObject
    {
        PoolingObject poolingObject = GetPoolingObject(fileName, hidePos, Quaternion.identity, pathCreate);
        return poolingObject.GetComponent<T>();
    }

    public T GetPoolingObject<T>(string fileName, Vector3 spawnPos, Quaternion spawnRot, string pathCreate) where T : PoolingObject
    {
        PoolingObject poolingObject = GetPoolingObject(fileName, spawnPos, spawnRot, pathCreate);
        return poolingObject.GetComponent<T>();
    }


    // 포지션, 로테이션, On을 한번에 사용할때
    void PoolingObjectInit(PoolingObject poolingObject, Vector3 spawnPos, Quaternion spawnRot)
    {
        poolingObject.transform.position = spawnPos;
        poolingObject.transform.rotation = spawnRot;
        poolingObject.On();
    }
}
public class PoolingPackage
{
    public List<PoolingObject> poolingObjects = new List<PoolingObject>();
    public float latestUsedTime; // 최근에 풀링이 사용된 시간 (사용한지 n분 지났으면 삭제 같은 기능 구현할때 씀)

    public PoolingObject GetPoolingObject()
    {
        latestUsedTime = Time.time;

        PoolingObject poolingObject = null;

        if (poolingObjects.Count > 0)    // 리스트에 풀링아이템이 있을때
        {
            for (int i = 0; i < poolingObjects.Count; i++)
            {
                if (!poolingObjects[i].onUse)   // 사용안하고 있는 풀링아이템
                {
                    poolingObject = poolingObjects[i];  // 풀링 return
                    poolingObjects.RemoveAt(i);         // 리스트에서 제거
                    break;
                }
            }
        }

        return poolingObject;
    }

    public void Return(PoolingObject poolingObject)
    {
        poolingObjects.Add(poolingObject);
    }
}