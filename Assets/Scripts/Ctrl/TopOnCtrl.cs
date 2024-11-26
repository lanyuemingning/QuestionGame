using AnyThinkAds.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopOnCtrl: MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    void Start()
    {
        ATSDKAPI.initSDK("a65f1418e613b4", "ab510b21388dd5720b375de95d540e154");
        ATSDKAPI.setLogDebug(false);

        DontDestroyOnLoad(gameObject);
    }


}
