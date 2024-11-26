using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class TipCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    Button BtnOK;
    [SerializeField]
    TextMeshProUGUI TxtOK, TxtDesc;


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
        BtnOK?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().HideUI("UITip");
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
        TxtOK.text = textManager.GetConvertText("Text_OK");
        TxtDesc.text = textManager.GetConvertText("Text_Privacy");
    }
}
