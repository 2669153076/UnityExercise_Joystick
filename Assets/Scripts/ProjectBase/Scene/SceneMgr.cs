using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    /// <summary>
    /// 加载场景 同步
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name,UnityAction fun)
    {
        SceneManager.LoadScene(name);

        fun?.Invoke();
    }

    /// <summary>
    /// 场景加载 异步
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsync(string name,UnityAction fun)
    {
        MonoMgr.GetInstance().StartCoroutine(LoadSceneIEnumertor(name, fun));
    }
    /// <summary>
    /// 加载场景携程
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneIEnumertor(string name,UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while(!ao.isDone)
        {
            //更新进度条
            EventCenter.GetInstance().EventTrigger("进度条更新",ao.progress);

            yield return ao.progress;
        }

        fun?.Invoke();
    }
}
