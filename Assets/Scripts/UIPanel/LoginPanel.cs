using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{



    // Start is called before the first frame update
    void Start()
    {
        UIManager.AddEventListener(GetControl<Button>("StartBtn"), EventTriggerType.PointerEnter, (data) =>
        {
            Debug.Log("进入");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitInfo()
    {
        Debug.Log("初始化");
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName); switch (btnName)
        {
            case "StartBtn":
                Debug.Log("开始");
                break;
            case "QuitBtn":
                Debug.Log("退出");
                break;
            case "SettingBtn":
                Debug.Log("设置");
                break;
        }
    }
}
