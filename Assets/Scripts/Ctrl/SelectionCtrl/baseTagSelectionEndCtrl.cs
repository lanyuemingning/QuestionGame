using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;
using System.Xml;
using System.Resources;

public class BaseTagSelectionEndCtrl : MonoBehaviour, IController
{
    //View
    public Image ImgAnswer;
    public TextMeshProUGUI TxtReturn, TxtRetry, TxtUnlock;
    public Button BtnReturn, BtnShare, BtnRetry, BtnUnlock, BtnLeft, BtnRight;
    public GameObject AllNode;

    //ViewData
    [SerializeField]
    public string returnTxt = "Text_ReturnBegin";
    [SerializeField]
    public string retryTxt = "Text_Retry";
    [SerializeField]
    public GameType gameType;

    public int nowTag, TagMax;

    //Model
    public SelectionModel m_Model;

    //Instance
    public LevelManager levelManager;
    public TextManager textManager;
    public ShareManager shareManager;

    //ViewData
    public string answerImgPath = "";

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        BtnUnlock = transform.Find("BtnUnlock").GetComponent<Button>();
        TxtUnlock = BtnUnlock.transform.Find("TxtUnlockAll").GetComponent<TextMeshProUGUI>();
        AllNode = GameObject.Find("AllNode");
        BtnLeft = AllNode.transform.Find("BtnLeft").GetComponent<Button>();
        BtnRight = AllNode.transform.Find("BtnRight").GetComponent<Button>();
        AllNode.SetActive(false);

        GetInstance();

        m_Model = this.GetModel<SelectionModel>(); //获取model
        m_Model.gameType = gameType;

        SetButtonOnclick();
        RegisterEvents();
        RefreshUI();
        SaveClear();
        this.GetUtility<UIUtility>().CloseGameUI(gameType);
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    public virtual void SetButtonOnclick()
    {
        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            OnBtnReturn();
        });

        BtnShare?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            shareManager.ShareImg(answerImgPath);
        });

        BtnRetry?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().CreateGameUI(gameType);
            OnBtnReturn();
        });

        BtnUnlock?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = ViewAll;
            TopOnADManager.Instance.ShowRewardAd();
        });

        BtnLeft?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowTag--;
            if (nowTag <= 1)
            {
                nowTag = 1;
            }
            ShowTag(nowTag);
        });

        BtnRight?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowTag++;
            if (nowTag >= TagMax)
            {
                nowTag = TagMax;
            }
            ShowTag(nowTag);
        });
    }

    public virtual void OnBtnReturn()
    {
        this.GetUtility<UIUtility>().CloseGameAnswerUI(gameType);
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    public virtual void RegisterEvents()
    {
        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public virtual void GetInstance()
    {
        levelManager = LevelManager.Instance;
        textManager = TextManager.Instance;
        shareManager = ShareManager.Instance;
    }

    public virtual void RefreshUI()
    {
        //int tag = m_Model.GetMostTag();
        int tag = this.GetUtility<SaveDataUtility>().GetLevelEndTag(gameType);
        //this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);

        ShowTag(tag);
    }

    public void ShowTag(int tag)
    {
        nowTag = tag;

        if(nowTag == 1)
        {
            BtnLeft.gameObject.SetActive(false);
        }
        else if(nowTag == 2)
        {
            BtnLeft.gameObject.SetActive(true);
        }
        else if(nowTag == TagMax - 1)
        {
            BtnRight.gameObject.SetActive(true);
        }
        else if(nowTag == TagMax)
        {
            BtnRight.gameObject.SetActive(false);
        }
        //Debug.Log("tag： " + tag);

        //TagData tagData = levelManager.GetTagData(GameType.Trail, tag);

        switch (gameType)
        {
            case GameType.Trail:
                answerImgPath = "trailanswer/";
                break;
            case GameType.Animal:
                answerImgPath = "animalanswer/";
                break;
            case GameType.Color:
                answerImgPath = "coloranswer/";
                break;
            case GameType.Job:
                answerImgPath = "jobanswer/";
                break;
            case GameType.Friend:
                answerImgPath = "friendanswer/";
                break;
            case GameType.Romance:
                answerImgPath = "romanceanswer/";
                break;
            case GameType.SuperHero:
                answerImgPath = "superheroanswer/";
                break;
        }

        string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
        languageStr = languageStr.ToUpper();
        answerImgPath += languageStr;
        answerImgPath = answerImgPath.ToLower();

        //answerImgPath = answerImgPath + tag;
        //ImgAnswer.sprite = Resources.Load<Sprite>(answerImgPath);
        ImgAnswer.sprite = ResourceManager.Instance.Load<Sprite>(answerImgPath, tag + "");

        TxtReturn.text = textManager.GetConvertText(returnTxt);
        TxtRetry.text = textManager.GetConvertText(retryTxt);
        TxtUnlock.text = textManager.GetConvertText("Text_ShowAllAnswer");
    }

    public virtual void SaveClear()
    {
        this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
    }

    public virtual void ViewAll()
    {
        AllNode.SetActive(true);
        BtnUnlock.gameObject.SetActive(false);
    }
}
