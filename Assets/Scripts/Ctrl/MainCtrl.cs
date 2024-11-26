using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class MainCtrl: MonoBehaviour, IController
{

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Start()
    {
        this.GetUtility<UIUtility>();
    }

}
