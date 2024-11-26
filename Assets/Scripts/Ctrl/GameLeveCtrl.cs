using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;
using static TopOnADManager;

public class GameLevelCtrl : MonoBehaviour, IController, ICanSendEvent
{
    //View
    [SerializeField]
    Button BtnReturn, BtnSet, BtnLeft, BtnRight, BtnUnlockAll;
    [SerializeField]
    TextMeshProUGUI TxtComplete, TxtCompeteNum, TxtPage, TxtUnlockAll;
    [SerializeField]
    List<Transform> page1, page2, page3;
    [SerializeField]
    Transform starList;

    //ViewData
    [SerializeField]
    string CompleteTxt;
    [SerializeField]
    int nowPage = 1, maxPage = 2;

    TextManager textManager;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        GetInstance();
        SetButtonOnclick();
        RegisterEvents();
        RefreshUI();
        SetPage();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    private void SetPage()
    {
        switch (nowPage)
        {
            case 1:
                foreach (var item in page1)
                {
                    item.gameObject.SetActive(true);
                }
                starList.gameObject.SetActive(true);
                foreach (var item in page2)
                {
                    item?.gameObject.SetActive(false);
                }
                foreach (var item in page3)
                {
                    item?.gameObject.SetActive(false);
                }
                break;
            case 2:
                foreach (var item in page1)
                {
                    item.gameObject.SetActive(false);
                }
                starList.gameObject.SetActive(false);

                foreach (var item in page2)
                {
                    item.gameObject.SetActive(true);
                }
                foreach (var item in page3)
                {
                    item.gameObject.SetActive(false);
                }
                break;
            case 3:
                foreach (var item in page3)
                {
                    item.gameObject.SetActive(true);
                }
                starList.gameObject.SetActive(false);

                foreach (var item in page2)
                {
                    item.gameObject.SetActive(false);
                }
                foreach (var item in page1)
                {
                    item.gameObject.SetActive(false);
                }
                break;
        }

        TxtPage.text = nowPage + " / " + maxPage;
    }

    void UnlockAll()
    {
        UnlockModel model = this.GetModel<UnlockModel>();
        model.unlockProgress++;
        SetUnlockAll();
        Debug.Log("unlock " + model.unlockProgress + " " + model.unlockMax);

        if (model.unlockProgress >= model.unlockMax)
        {
            for (int i = 1; i < 12; i++)
            {
                this.GetUtility<SaveDataUtility>().SaveLevelUnlock(i);
            }

            model.unlockProgress = model.unlockMax;
            this.SendEvent<UnlockLevelEvent>();
        }
        
    }

    void SetUnlockAll()
    {
        UnlockModel model = this.GetModel<UnlockModel>();
        TxtUnlockAll.text = textManager.GetConvertText("Text_UnlockAll") + "(" + model.unlockProgress + "/" + model.unlockMax + ")";

        int unlockNum = 0;
        for(int i = 1; i < 12; i++)
        {
            if(this.GetUtility<SaveDataUtility>().GetLevelUnlock(i))
            {
                unlockNum++;
            }
        }
        //BtnUnlockAll.gameObject.SetActive(unlockNum < 10);
        BtnUnlockAll.gameObject.SetActive(false);
    }
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UIStart");
            this.GetUtility<UIUtility>().HideUI("UILevelSelect");
        });

        BtnSet?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UISet");
        });

        BtnLeft?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowPage--;
            if(nowPage < 1)
            {
                nowPage = 1;
            }
            SetPage();
        });

        BtnRight?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowPage++;
            if(nowPage > maxPage)
            {
                nowPage = maxPage;
            }
            SetPage();
        });

        BtnUnlockAll?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = UnlockAll;
            TopOnADManager.Instance.ShowRewardAd();
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

        this.RegisterEvent<UnlockLevelEvent>(e =>
        {
            SetUnlockAll();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    void GetInstance()
    {
        textManager = TextManager.Instance;
        SetUnlockAll();

    }

    void RefreshUI()
    {
        //TxtComplete.text = textManager?.GetConvertText(CompleteTxt);
        //TxtCompeteNum.text = this.GetUtility<SaveDataUtility>().GetClearLevelNum() + "/" + GameConst.totalLeveNum;

    }

}
