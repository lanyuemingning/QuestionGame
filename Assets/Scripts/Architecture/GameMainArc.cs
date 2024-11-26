using DG.Tweening;
using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    protected override void Init()
    {
        ResourceManager.Instance.Init();

        RegisterModels();
        UILoadingCtrl.Instance.ImgLoading.DOFillAmount(0.1f, 1f);
        RegisterUtilitys();
        UILoadingCtrl.Instance.ImgLoading.DOKill();
        UILoadingCtrl.Instance.ImgLoading.DOFillAmount(0.2f, 1f);
        RegisterSystems();
        UILoadingCtrl.Instance.ImgLoading.DOKill();
        UILoadingCtrl.Instance.ImgLoading.DOFillAmount(0.3f, 1f);
        CreateInstance();
        UILoadingCtrl.Instance.ImgLoading.DOKill();
        UILoadingCtrl.Instance.ImgLoading.DOFillAmount(0.4f, 1f);
        //GetUtility<SaveDataUtility>().ClearSaveLevel(); //清理存档
        //GetUtility<SaveDataUtility>().ClearAllEndTag(); //清理存档
        //Debug.Log(GetUtility<LanguageUtility>().GetSystemLanguage());
        //PlayerPrefs.DeleteKey("g_ClearLevelUnlock");
        //for (int i = 0; i < 5; i++)
        //{
        //    PlayerPrefs.DeleteKey("g_LevelTrailTag" + i);
        //}

        AudioKit.PlayMusic("resources://Sound/bgm");
        GameObject go = Camera.main.gameObject;

        go.AddComponent<GameMainCtrl>();

    }

    void RegisterModels()
    {
        RegisterModel(new SelectionModel());
        RegisterModel(new SurvivalModel());
        RegisterModel(new UnlockModel());
        RegisterModel(new ScoreModel());
    }

    void RegisterUtilitys()
    {
        RegisterUtility(new SaveDataUtility());
        RegisterUtility(new UIUtility());
        RegisterUtility(new LevelUtility());
        RegisterUtility(new LanguageUtility());
    }

    void RegisterSystems()
    {
    }

    void CreateInstance()
    {

        TextManager textManager = TextManager.Instance;
        string languageStr = GetUtility<SaveDataUtility>().GetSelectLanguage();
        if (languageStr == "-1")
        {
            languageStr = GetUtility<LanguageUtility>().GetSystemLanguage();
            GetUtility<SaveDataUtility>().SaveSelectLanguage(languageStr);
        }
        //Debug.Log("languageStr " + languageStr);

        textManager.ReadTextCfg(languageStr);

        LevelManager levelManager = LevelManager.Instance;
        levelManager.ReadAllCfg();

        //ADManager aDManager = ADManager.Instance;
        //aDManager.InitializeAds();

        ShareManager shareManager = ShareManager.Instance;
        AnalyticsManager analyticsManager = AnalyticsManager.Instance;
        TopOnADManager topOnADManager = TopOnADManager.Instance;
    }


}
