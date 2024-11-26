using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class ReturnTipCtrl: MonoBehaviour, IController, ICanSendEvent
{
    //View
    [SerializeField]
    Button BtnConfirm, BtnCancel;
    [SerializeField]
    TextMeshProUGUI TxtReturn, TxtDesc1, TxtDesc2, TxtCancel, TxtConfirm;


    //Instance
    TextManager textManager;
    [SerializeField]
    int gameType = 0;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        this.GetUtility<SaveDataUtility>().SetPrivacyTip(1);
        SetButtonOnclick();
        RegisterEvents();
        GetInstance();
        RefreshUI();
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnConfirm?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.SendEvent<CloseGameUIEvent>();
            this.GetUtility<UIUtility>().CloseUI("UIReturnTip");
        });  

        BtnCancel?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().CloseUI("UIReturnTip");
        });       
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    void RegisterEvents()
    {
        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    void GetInstance()
    {
        textManager = TextManager.Instance;
    }

    void RefreshUI()
    {
        TxtReturn.text = textManager.GetConvertText("Text_Return");
        TxtDesc1.text = textManager.GetConvertText("Text_ReturnMenu");
        TxtDesc2.text = textManager.GetConvertText("Text_ReturnDesc");
        TxtCancel.text = textManager.GetConvertText("Text_Cancel");
        TxtConfirm.text = textManager.GetConvertText("Text_Confirm");
    }
}
