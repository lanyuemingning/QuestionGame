using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class PrivacyCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    Button BtnOK;


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
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnOK?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().HideUI("UIPrivacy");
        });       
    }

}
