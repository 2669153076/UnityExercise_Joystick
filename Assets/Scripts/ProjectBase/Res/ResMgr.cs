using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    /// <summary>
    /// 资源加载 同步
    /// </summary>
    /// <param name="name"></param>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);

        //如果对象为GameObject类型 则将其实例化后 再返回
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }

    /// <summary>
    /// 资源加载 异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void LoadAsync<T>(string name,UnityAction<T> callback) where T : Object
    {
        MonoMgr.GetInstance().StartCoroutine(LoadAsyncIEnumerator<T>(name,callback));
    }
    /// <summary>
    /// 资源加载 异步 协程
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerator LoadAsyncIEnumerator<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);

        yield return r;

        if(r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }

}
