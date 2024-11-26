using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class GameMainCtrl : MonoBehaviour, IController
{

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        //if(this.GetUtility<SaveDataUtility>().GetPrivacyTip() == 0)
        //{
        //    this.GetUtility<UIUtility>().OpenUI("UITip");
        //}
    }

}
