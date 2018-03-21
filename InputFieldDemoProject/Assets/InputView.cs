#pragma warning disable 3001,3002,3003,3005,3008,3009,3024
using UnityEngine;
#if USE_PROTO
#elif USE_JSON
using use_json;
#endif

using UnityEngine.UI;

public class InputView : MonoBehaviour
{
    public Text txt_content;
    public InputField input_field; // only use for UNITY_EDITOR test
    public Button btnSendInput;
    public Button btnClearLog;

    //调用输入法相关
    private string inputContent;//不可删除！！
    private TouchScreenKeyboard keyBoardAndroid;
    private bool _hasClickedInputTopMask = false;//是否点过顶部遮罩

    private bool keyboardVisable = true;
    private bool keyboardHideInput = false;
    private bool hasEditInput = false;

    private RectTransform _rectTransfrom;
    private float screenHeight = 0;


    public void Awake()
    {
        txt_content.text = "";

        _rectTransfrom = this.GetComponent<RectTransform>();
        InputTool.initScrrenSize(this.transform);
        screenHeight = InputTool.ScreenSize.y;

        setInputFieldPos(true);

        //清理输入框
        keyboardVisable = true;
        keyboardHideInput = false;
        //hasKeyboardObj = true;
        hasEditInput = false;

        this.input_field.text = "";

        btnSendInput.onClick.AddListener(() => { OnClickBtnSendChatMsg(btnSendInput.gameObject); });
        btnClearLog.onClick.AddListener(() => { resetContent(); });
    }

    private void resetContent()
    {
        txt_content.text = "";
    }

    private void setInputFieldPos(bool showPos)
    {
#if UNITY_EDITOR
#else
AddLog("非编辑器，把输入框挪远 showPos="+showPos);
        showPos = false;
#endif
        input_field.GetComponent<RectTransform>().anchoredPosition = showPos ? new Vector2(-54, -164) : new Vector2(-54, 10000);
    }

    /// <summary>
    /// 发送聊天消息
    /// </summary>
    /// <param name="text"></param>
    public void SendChatMessage(string text)
    {
        AddLog("完成发送 text="+text);
        this.input_field.text = "";
        //完成输入的提交
        ActiveTopInputMask(false);
    }

    public void ActiveTopInputMask(bool active)
    {
        //Logger.Log("显示顶部输入法遮挡？" + active + ",点过顶部遮罩？" + _hasClickedInputTopMask);
        _hasClickedInputTopMask = false;
    }

    public void Update()
    {
        //2017-9-30 yanru 备注：测试发现，iphone输入法在最新unity2017出的包如下条件就可以满足输入处理了，后面的#if全未生效
        if (keyboardVisable != TouchScreenKeyboard.visible || keyboardHideInput != TouchScreenKeyboard.hideInput)
        {
            AddLog("输入结束：TouchScreenKeyboard.visible=" + TouchScreenKeyboard.visible
                + ",TouchScreenKeyboard.hideInput=" + TouchScreenKeyboard.hideInput
                + ",keyBoardAndroid=" + (keyBoardAndroid == null)
                + ",hasEditInput?" + hasEditInput);

            keyboardVisable = TouchScreenKeyboard.visible;
            keyboardHideInput = TouchScreenKeyboard.hideInput;

            //2017-9-30 在iphone测试发现 点done的时候，这两个条件必须同时满足，
            //否则，如果点输入法空白处 或者点取消哦，hideInput会为true，但TouchScreenKeyboard.visible却为true，此时不应该处理输入
            if (keyboardVisable == false)//hidepunt条件不是必须的.跟是否结束输入无关系 && keyboardHideInput == false)
            {
                AddLog("此时文本=" + this.input_field.text + ",hasEditInput?" + hasEditInput
                + ",点过顶部遮罩？" + _hasClickedInputTopMask + ",inputString=" + Input.inputString + ",keyBoardAndroid=" + (keyBoardAndroid == null ? "NULL" : keyBoardAndroid.text));
                string inputContent = this.input_field.text; //注意：假如输入法组件 未显示在屏幕中，input_field.text内容为空，此时只能通过keyBoardAndroid去获取输入的文本
                if (keyBoardAndroid != null)
                {
                    inputContent = keyBoardAndroid.text;
                }
                if (inputContent != "" &&
                    false == _hasClickedInputTopMask) //点击过输入法以外的顶部遮罩则按“取消”处理
                {
                    SendChatMessage(inputContent);
                    return;
                }
                else
                {
                    ActiveTopInputMask(false);//_hasClickedInputTopMask 为true的时候，可以修改为 清空输入框文本
                }
            }
            else
            {
                hasEditInput = false;
            }
        }

        //备注：一定要在 OnUpdate 检查，保证先结束输入法处理，后重置UI通知。
        InputTool.keyBoardUpdate(_rectTransfrom, screenHeight);
    }

    private void AddLog(string testLog)
    {
        txt_content.text += testLog + "\n";
    }

    /// <summary>
    /// 点击发送聊天消息
    /// </summary>
    /// <param name="go"></param>
    public void OnClickBtnSendChatMsg(GameObject go)
    {
        Debug.Log("点击发送按钮 text=" + this.input_field.text);
#if UNITY_EDITOR
        string text = this.input_field.text.Trim();
        if (text == "")
        {
            AddLog("输入为空");
            //     ExhibitionUIProxy.instance.ShowScreenCenterTip("LEGION_CHAT_MSG_NULL");//输入聊天为空了
            return;
        }

        SendChatMessage(text);

#elif UNITY_ANDROID || UNITY_IOS
        
        ActiveTopInputMask(true);//输入聊天内容 前 遮挡输入

        keyBoardAndroid = TouchScreenKeyboard.Open(inputContent);

//#elif UNITY_IOS //打开iphone输入框
//        //第一个参数 默认显示 test
//        //第二个参数 设置输入框类型，这里为默认，什么都可以输入
//        keyboard = iPhoneKeyboard.Open("test", iPhoneKeyboardType.Default);
#endif
    }

    private string _lastMsgContent = "";

    /// <summary>
    /// 输入法弹出
    /// </summary>
    public void OnUIRootMoveUp(object data)
    {
        float moveUp = (float)data;

        ////    CanvasCtrl.instance.OpenLoading(LoadingType.NoCircle);
        //    DOTweenCom tMove = DOTweenCom.DOLocalMoveY(TweenKillType.OnComplete, transform, moveUp, 0.5f);
        //    tMove.tween.DOTweenSetEase(DOTweenEase.Linear);
        //    tMove.tween.DOTweenOnComplete(delegate
        //    {
        //       // CanvasCtrl.instance.CloseLoading();
        //    });

        Vector2 pos = transform.GetComponent<RectTransform>().anchoredPosition;
        pos.y = moveUp;
        transform.GetComponent<RectTransform>().anchoredPosition = pos;

        ActiveTopInputMask(true);
    }
}