using AppsFlyerSDK;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppsflyerCtrl: MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    void Start()
    {
        AppsFlyer.initSDK("nqQi2pBHUJG4CiW75VJXUW", "com.Baolie.Selection");
        AppsFlyer.startSDK();

        AppsFlyer.setCustomerUserId("Baolie");

        AppsFlyer.setIsDebug(false);
        DontDestroyOnLoad(gameObject);
    }


}
