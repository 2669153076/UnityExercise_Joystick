using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public GameObject fatherObj;    //父对象
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;

        poolList = new List<GameObject>() {  };
        PushObj(obj);
    }

    /// <summary>
    /// 存
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj( GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        //设置父对象
        obj.transform.parent = fatherObj.transform;
    }
    /// <summary>
    /// 取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}

public class PoolMgr : BaseManager<PoolMgr>
{
    //容器
    public Dictionary<string,PoolData> poolDir = new Dictionary<string, PoolData>();

    //根节点
    private GameObject poolObj;

    /// <summary>
    /// 取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void GetObj(string name,UnityAction<GameObject> callback)
    {
        if (poolDir.ContainsKey(name) && poolDir[name].poolList.Count > 0)
        {
            callback(poolDir[name].GetObj());
        }
        else
        {
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //obj.name = name;

            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callback(o);
            });
        }
        //return obj;
    }

    /// <summary>
    /// 存
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(string name, GameObject obj)
    {
        //设置根节点
        if(poolObj==null)
            poolObj = new GameObject("Pool"+obj.name);

        if (poolDir.ContainsKey(name))
        {
            poolDir[name].PushObj(obj);
        }
        else
        {
            poolDir.Add(name, new PoolData(obj,poolObj) );
        }
    }

    /// <summary>
    /// 清空 切场景时使用
    /// </summary>
    public void Clear()
    {
        poolDir.Clear();
        poolObj = null;
    }
}
