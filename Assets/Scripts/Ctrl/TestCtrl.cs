using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestCtrl : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        this.GetUtility<UIUtility>().CreateGameUI(GameDefine.GameType.Trail);
    }

   
}
