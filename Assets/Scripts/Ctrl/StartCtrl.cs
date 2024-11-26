using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class StartCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    Button BtnStart, BtnSet, BtnAD, BtnShare, BtnSave;
    [SerializeField]
    TextMeshProUGUI TxtStart;
    [SerializeField]
    Sprite logoZH, logoEN;
    [SerializeField]
    Image logoImg;
    [SerializeField]
    GameObject contentNode;

    //ViewData
    [SerializeField]
    string startText;

    //Instance
    TextManager textManager;
    TopOnADManager adManager;
    ShareManager shareManager;
    [SerializeField]
    int gameType = 0;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        //TxtStart.font = null;
        Debug.Log("初始化开始");
        SetButtonOnclick();
        RegisterEvents();
        GetInstance();
        RefreshUI();
        //ResourceManager.Instance.LoadFont();
    }


    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnStart?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().HideUI("UIStart");

            CheckFirstEnter();

        });

        BtnSet?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UISet");
        });

        BtnAD?.onClick.AddListener(() =>
        {
            adManager.ShowRewardAd();
        });

        BtnShare?.onClick.AddListener(() =>
        {
            shareManager.ShareImg("Sprite/Trail/TrailAnswer/1");
        });

        BtnSave?.onClick.AddListener(() =>
        {
            this.GetUtility<SaveDataUtility>().SaveLevel(gameType);
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

        this.RegisterEvent<ShowStartEvent>(e =>
        {
            ShowContent();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    void GetInstance()
    {
        textManager = TextManager.Instance;
        adManager = TopOnADManager.Instance;
        shareManager = ShareManager.Instance;

        //adManager.splashCompleteAction += ShowContent;
        adManager.LoadAD();
        StartCoroutine(CheckLoadSplash());
    }

    IEnumerator CheckLoadSplash()
    {
        Debug.Log("CheckLoadSplash ");
        yield return new WaitForSeconds(5f);
        Debug.Log("CheckLoadSplash 1");

        if (adManager.beginLoadSplash)
        {

        }
        else
        {
            ShowContent();
        }
    }

    void RefreshUI()
    {
        TxtStart.text = textManager.GetConvertText(startText);

#if UNITY_EDITOR
        ShowContent();
#endif

        LanguageType languageType = this.GetUtility<SaveDataUtility>().GetSelectLanguageType();
        switch(languageType)
        {
            case LanguageType.en:
                logoImg.sprite = logoEN;
                break;
            default: 
                logoImg.sprite = logoZH; 
                break;
        }
    }


    void CheckFirstEnter()
    {
        this.GetUtility<UIUtility>().ShowUI("UILevelSelect");
        //if (this.GetUtility<SaveDataUtility>().GetLevelClear((int)GameType.Trail))
        //{
        //    this.GetUtility<UIUtility>().ShowUI("UILevelSelect");
        //}
        //else
        //{

        //    this.GetUtility<UIUtility>().CreateGameUI(GameType.Trail);
        //}
    }

    void ShowContent()
    {
        TopOnADManager.Instance.beginLoadSplash = true;

        contentNode.SetActive(true);
    }
}
