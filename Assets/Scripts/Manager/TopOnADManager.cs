using AnyThinkAds.Api;
using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

[MonoSingletonPath("[Analytics]/TopOnADManager")]
public class TopOnADManager: MonoSingleton<TopOnADManager>, ICanGetUtility, ICanSendEvent
{

    string mPlacementId_rewardvideo_all = "b65f14572ada01";
    string mPlacementId_interstitial_all = "b66023770f1f6e";
    string mPlacementId_native_all = "b6604dcc178e8a";
    string mPlacementId_splash_all = "b6604dcd293d1c";
    string showingScenario = "f65f152173a5c1";

    public delegate void RewardAction();
    public delegate void SplashCompleteAction();

    public RewardAction rewardAction;
    public SplashCompleteAction splashCompleteAction;

    public bool beginLoadSplash;

    private void Start()
    {
        AddAutoLoadAdPlacementID();;
    }

    public void LoadAD()
    {
        LoadRewardAD();
        LoadInterstitialAd();
        LoadBannerAd();
        LoadSplashAD();
    }

    public void LoadRewardAD()
    {
        ATSDKAPI.setCustomDataForPlacementID(new Dictionary<string, string> { { "placement_custom_key", "placement_custom" } }, mPlacementId_rewardvideo_all);

        Dictionary<string, string> jsonmap = new Dictionary<string, string>();
        jsonmap.Add(ATConst.USERID_KEY, "test_user_id");
        jsonmap.Add(ATConst.USER_EXTRA_DATA, "test_user_extra_data");


        ATRewardedVideo.Instance.loadVideoAd(mPlacementId_rewardvideo_all, jsonmap);
    }

    public void LoadInterstitialAd()
    {

        Dictionary<string, object> jsonmap = new Dictionary<string, object>();

        ATInterstitialAd.Instance.loadInterstitialAd(mPlacementId_interstitial_all, jsonmap);
    }

    public void LoadBannerAd()
    {
        Dictionary<string, object> jsonmap = new Dictionary<string, object>();
        //配置Banner要展示的宽度，高度，是否使用pixel为单位（只针对iOS有效，Android 使用pixel为单位），注意不同平台的横幅广告有一定限制，例如配置的穿山甲横幅广告640*100，为了能填充完屏幕宽，计算高度H = (屏幕宽 *100)/640；那么在load的extra的size为（屏幕宽：H）。
        ATSize bannerSize = new ATSize(1080, 900, true);
        jsonmap.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSizeStruct, bannerSize);
        jsonmap.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraInlineAdaptiveWidth, bannerSize.width);
        jsonmap.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraInlineAdaptiveOrientation, ATBannerAdLoadingExtra.kATBannerAdLoadingExtraInlineAdaptiveOrientationCurrent);

        ATBannerAd.Instance.loadBannerAd(mPlacementId_native_all, jsonmap);

        
    }

    public void LoadSplashAD()
    {
        //ATSplashAd.Instance.loadSplashAd(mPlacementId_splash_all, new Dictionary<string, object>());
        //ATSplashAd.Instance.client.onAdLoadFailureEvent += OnAdLoadSplashFailed;
        //ATSplashAd.Instance.client.onAdCloseEvent += onAdClose;
    }

    public void AddAutoLoadAdPlacementID()
    {
        ATRewardedVideo.Instance.client.onAdLoadEvent += OnAdLoad;
        ATRewardedVideo.Instance.client.onAdLoadFailureEvent += OnAdLoadFailure;
        ATRewardedVideo.Instance.client.onRewardEvent += OnReward;
        ATRewardedVideo.Instance.client.onAdVideoStartEvent += OnStart;
        ATRewardedVideo.Instance.client.onAdVideoCloseEvent += OnClosed;

        ATInterstitialAd.Instance.client.onAdLoadEvent += OnAdLoad;
        ATInterstitialAd.Instance.client.onAdLoadFailureEvent += OnInterstitialAdLoadFailure;
        ATInterstitialAd.Instance.client.onAdVideoEndEvent += OnReward;
        ATInterstitialAd.Instance.client.onAdVideoFailureEvent += OnReward;
        ATInterstitialAd.Instance.client.onAdVideoStartEvent += OnStart;
        ATInterstitialAd.Instance.client.onAdShowEvent += OnInterstitialClosed;

        ATBannerAd.Instance.client.onAdLoadEvent += OnAdLoad;
        ATBannerAd.Instance.client.onAdLoadFailureEvent += OnBannerAdLoadFailure;
        ATBannerAd.Instance.client.onAdImpressEvent += OnStart;


        //ATSplashAd.Instance.client.onAdLoadEvent += OnSplashAdLoad;
        //ATSplashAd.Instance.client.onAdLoadTimeoutEvent += OnSplashAdComplete;
        //ATSplashAd.Instance.client.onAdLoadFailureEvent += OnSplashAdComplete;
        //ATSplashAd.Instance.client.onAdCloseEvent += OnSplashAdComplete;

        //string[] jsonList = { mPlacementId_rewardvideo_all };
        //ATRewardedVideo.Instance.addAutoLoadAdPlacementID(jsonList);
    }

    public void ShowRewardAd()
    {
#if UNITY_EDITOR
        if(rewardAction != null)
        {
            rewardAction();
        }
        return;
#endif
        if(ATRewardedVideo.Instance.hasAdReady(mPlacementId_rewardvideo_all))
        {
            Debug.Log("播放广告");
            Dictionary<string, string> jsonmap = new Dictionary<string, string>();
            jsonmap.Add(AnyThinkAds.Api.ATConst.SCENARIO, showingScenario);
            ATRewardedVideo.Instance.showAd(mPlacementId_rewardvideo_all, jsonmap);

        }
        else
        {
            LoadRewardAD();
            //ShowAd();
        }
    }
    public void ShowInterstitialAd()
    {
        if(ATInterstitialAd.Instance.hasInterstitialAdReady(mPlacementId_interstitial_all))
        {
            Debug.Log("播放广告");
            ATInterstitialAd.Instance.showInterstitialAd(mPlacementId_interstitial_all);
        }
        else
        {
            LoadInterstitialAd();
            //ShowAd();
        }
    }

    public void ShowBannerAd()
    {
        //ATBannerAd.Instance.showBannerAd(mPlacementId_native_all, ATBannerAdLoadingExtra.kATBannerAdShowingPisitionBottom);
        ATRect arpuRect = new ATRect(75, 1520, 1080, 900, true);
        ATBannerAd.Instance.showBannerAd(mPlacementId_native_all, arpuRect);

    }

    public void RemoveBannerAd()
    {
        ATBannerAd.Instance.cleanBannerAd(mPlacementId_native_all);
        LoadBannerAd();
    }

    public void OnAdLoadFailure(object sender, ATAdEventArgs erg)
    {
        LoadRewardAD();
    }

    public void OnInterstitialAdLoadFailure(object sender, ATAdEventArgs erg)
    {
        LoadInterstitialAd();
    }

    public void OnBannerAdLoadFailure(object sender, ATAdEventArgs erg)
    {
        LoadBannerAd();
    }

    public void OnAdLoad(object sender, ATAdEventArgs erg)
    {
        Debug.Log("OnAdLoad " + erg.callbackInfo.adsource_id);
    }

    public void OnSplashAdLoad(object sender, ATAdEventArgs erg)
    {
        //
        if(erg.isTimeout)
        {
        }
        else
        {
            StartCoroutine(OpenStart(erg));
        }

        //splashCompleteAction();
    }

    IEnumerator OpenStart(ATAdEventArgs erg)
    {
        yield return new WaitForEndOfFrame();
        ATSplashAd.Instance.showSplashAd(mPlacementId_splash_all, new Dictionary<string, object>());

        Debug.Log("erg.isTimeout " + erg.isTimeout);
        this.SendEvent<ShowStartEvent>();
    }

    public void OnSplashAdComplete(object sender, ATAdEventArgs erg)
    {
        Debug.Log("OnSplashAdComplete");
    }

    public void OnClosed(object sender, ATAdEventArgs erg)
    {
        Debug.Log("OnClosed " + erg.callbackInfo.adsource_id);
        LoadRewardAD();
    }

    public void OnInterstitialClosed(object sender, ATAdEventArgs erg)
    {
        Debug.Log("OnClosed " + erg.callbackInfo.adsource_id);
        LoadInterstitialAd();
    }

    public void OnStart(object sender, ATAdEventArgs erg)
    {
        Debug.Log("StartAD " + erg.callbackInfo.adsource_id);
    }

    public void OnReward(object sender, ATAdEventArgs erg)
    {
        Debug.Log("广告激励");
        StartCoroutine(OnReward());
    }

    public IEnumerator OnReward()
    {
        yield return new WaitForEndOfFrame();

        if (rewardAction != null)
        {
            rewardAction();
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

}
