using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

[MonoSingletonPath("[Analytics]/AnalyticsManager")]
public class AnalyticsManager : MonoSingleton<AnalyticsManager>, ICanGetUtility, ICanSendEvent
{

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        ConsentGiven();
    }
    void ConsentGiven()
    {
        AnalyticsService.Instance.StartDataCollection();
    }

    public void SendServerEvent(string eventName, Dictionary<string, object> parameters)
    {
        AnalyticsService.Instance.CustomData(eventName, parameters);
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

}
