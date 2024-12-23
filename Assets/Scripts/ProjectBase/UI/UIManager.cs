using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}

/// <summary>
/// UI管理器
/// 管理所有显示的面板
/// 提供给外部 显示和隐藏的接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    private RectTransform canvas;   
    private Transform bot;  //底层
    private Transform mid;  //中层
    private Transform top;  //上层
    private Transform system;   //系统层

    public Dictionary<string ,BasePanel> panelDic = new Dictionary<string ,BasePanel>();

    public UIManager()
    {
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UIPanel/Canvas");
        canvas = obj.transform as RectTransform;

        bot = canvas.Find("bot");
        mid = canvas.Find("mid");
        top = canvas.Find("top");
        system = canvas.Find("system");

        GameObject.DontDestroyOnLoad(obj);
        GameObject.DontDestroyOnLoad(ResMgr.GetInstance().Load<GameObject>("UIPanel/EventSystem"));
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">所要显示的层级</param>
    /// <param name="callback">面板创建完成后要做什么</param>
    public void ShowPanel<T>(string panelName,E_UI_Layer layer,UnityAction<T> callback) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowSelf();
            if (callback != null)
                callback(panelDic[panelName] as T);
            return;
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UIPanel/" + panelName, (obj) =>
        {
            Transform father = GetLayerFather(layer);

            obj.transform.SetParent(father, false);
            //设置相对位置和大小
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //获取预制体身上挂载的脚本
            T panel =  obj.GetComponent<T>();
            //处理面板创建完成后的事情
            if(callback != null)
                callback(panel);

            panel.ShowSelf();

            //存储面板
            panelDic.Add(panelName, panel);
        });
    }
    /// <summary>
    /// 获取面板层级父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    private Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return  this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideSelf();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);

        }
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callback">事件的响应函数</param>
    public static void AddEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger ==null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}
