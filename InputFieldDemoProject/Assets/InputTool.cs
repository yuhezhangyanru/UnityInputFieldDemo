using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InputTool
{
    private static Vector2 _screenSize = Vector2.zero;
    public static Vector2 ScreenSize
    {
        get
        {
            return _screenSize;
        }
        set
        {
            _screenSize = value;
        }
    }
    public static Vector2 initScrrenSize(Transform view)
    {
        float width = view.GetComponent<RectTransform>().rect.width;
        float height = view.GetComponent<RectTransform>().rect.height;
        //Logger.LogError("", "initScrrenSize view=" + view.transform.name + ",width=" + width + ",height=" + height);
        _screenSize = new Vector2(width, height);
        return _screenSize;
    }


    //输入框变化相关
    //检测输入框变化
    public static bool testVisable = false;
    public static bool visible = false;
    private static bool hideInput = false;
    private static float inputHeight = 0f;
    private static int usefulInputHeight = 0;//作为有效的输入法高度

    private static Vector2 top = new Vector2();
    private static float yMin = 0f;
    private static float yMax = 0f;
    private static Vector2 position = new Vector2(float.MaxValue, float.MaxValue);
    private static string rectInfo = "";
    private static Rect testRect = new Rect(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MaxValue, float.MaxValue));

    private static bool getKeyboardVisable
    {
        get
        {
            //return testVisable;// 测试参数
            return TouchScreenKeyboard.visible;
        }
        set
        {
            testVisable = value;
        }
    }

    //在有输入法的页面，Update中执行检查，输入法窗口是否弹出以及当前输入法窗口有多高
    /// <summary>
    /// 刷新窗口高度
    /// </summary>
    /// <param name="moveUpValue"></param>
    public static void keyBoardUpdate(RectTransform viewRect, float moveUpValue)
    {
        if (visible != getKeyboardVisable ||
            hideInput != TouchScreenKeyboard.hideInput ||
            inputHeight != TouchScreenKeyboard.area.height ||
            top != TouchScreenKeyboard.area.min ||
            yMin != TouchScreenKeyboard.area.yMin ||
            yMax != TouchScreenKeyboard.area.yMax ||
            rectInfo != TouchScreenKeyboard.area.ToString() ||
            position != TouchScreenKeyboard.area.position ||
            testRect != TouchScreenKeyboard.area)
        {
            float screenHeight = viewRect.rect.height / 3f;


            visible = getKeyboardVisable;
            hideInput = TouchScreenKeyboard.hideInput;
            inputHeight = TouchScreenKeyboard.area.height;
            if (inputHeight > 0)
            {
                usefulInputHeight = (int)inputHeight;
            }

            top = TouchScreenKeyboard.area.min;
            yMin = TouchScreenKeyboard.area.yMin;
            yMax = TouchScreenKeyboard.area.yMax;
            rectInfo = TouchScreenKeyboard.area.ToString();
            position = TouchScreenKeyboard.area.position;
            testRect = TouchScreenKeyboard.area;

            //上移高度
            float moveUp = usefulInputHeight;

            if (visible)
            {
                if (moveUp == 0f)
                    moveUp = screenHeight;

                //   Logger.Log("上移 UI！" + moveUp);
                //   GameFacade.instance.SendEvent(MainUIEvent.ON_UI_ROOT_MOVE_UP, moveUp);
                //TODO
            }
            else
            {
                //   Logger.Log("重置 UI跟节点位置！");
                //    GameFacade.instance.SendEvent(MainUIEvent.ON_UI_ROOT_RESET, null);
                //TODO
            }
        }
    }
}