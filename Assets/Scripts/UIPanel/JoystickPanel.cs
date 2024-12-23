using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum E_JoystickType
{
    Normal,
    CanChangePos,
    CanMove
}

public class JoystickPanel : BasePanel
{
    public float maxL = 180;

    public E_JoystickType joystickType = E_JoystickType.Normal;

    private Image touchRectImage;
    private Image bkImage;  //摇杆背景
    private Image controlImage; //摇杆
    // Start is called before the first frame update
    void Start()
    {
        touchRectImage = GetControl<Image>("TouchRectImage");
        bkImage = GetControl<Image>("BKImage");
        controlImage = GetControl<Image>("ControlImage");

        UIManager.AddEventListener(touchRectImage, EventTriggerType.PointerDown, PointerDown);
        UIManager.AddEventListener(touchRectImage, EventTriggerType.PointerUp, PointerUp);
        UIManager.AddEventListener(touchRectImage, EventTriggerType.Drag, Drag);

        //可变位置摇杆 默认隐藏
        if (joystickType != E_JoystickType.Normal)
            bkImage.gameObject.SetActive(false);
    }

    private void PointerDown(BaseEventData eventData)
    {
        if (joystickType != E_JoystickType.Normal)
        {
            //可变位置摇杆
            //按下显示
            bkImage.gameObject.SetActive(true);
            //点击屏幕的位置显示
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(touchRectImage.rectTransform, (eventData as PointerEventData).position, (eventData as PointerEventData).pressEventCamera, out localPos);
            bkImage.transform.localPosition = localPos;
        }
    }
    private void PointerUp(BaseEventData eventData)
    {
        controlImage.transform.localPosition = Vector3.zero;
        EventCenter.GetInstance().EventTrigger("Joystick", Vector2.zero);

        //可变位置摇杆 抬起隐藏
        if (joystickType != E_JoystickType.Normal)

            bkImage.gameObject.SetActive(false);
    }
    private void Drag(BaseEventData eventData)
    {
        Vector2 localPos;

        //参数一：想要改变位置的对象 的父对象
        //参数二：当前屏幕鼠标位置
        //参数三：UI用的摄像机
        //参数四：相对坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bkImage.rectTransform, (eventData as PointerEventData).position, (eventData as PointerEventData).pressEventCamera, out localPos);

        //更新位置
        controlImage.transform.localPosition = localPos;

        if (localPos.magnitude > maxL)
        {
            if (joystickType == E_JoystickType.CanMove)
            {
                //超出多少，背景图动多少
                bkImage.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - maxL));
            }


            controlImage.transform.localPosition = localPos.normalized * maxL;
        }

        EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", localPos.normalized);
    }
}
