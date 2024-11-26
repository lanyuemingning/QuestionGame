using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class BaseTagAddEndCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    public TextMeshProUGUI TxtReturn;
    public TextMeshProUGUI TxtSurvival, TxtDesc, TxtRetry;
    [SerializeField]
    public Button BtnReturn, BtnShare, BtnRetry;
    [SerializeField]
    List<int> starCheck = new List<int>();
    [SerializeField]
    Transform pos_1, pos_2;
    [SerializeField]
    List<int> checkNums = new List<int>();

    //ViewData
    [SerializeField]
    public string returnTxt = "Text_ReturnBegin";
    public string survivalTxt = "Text_Survival";
    public string retryTxt = "Text_Retry";

    [SerializeField]
    public GameType gameType;

    //Model
    public ScoreModel m_Model;

    //Instance
    public LevelManager levelManager;
    public TextManager textManager;
    public ShareManager shareManager;

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        GetComponents();
        GetInstance();

        m_Model = this.GetModel<ScoreModel>(); //获取model
        m_Model.gameType = gameType;

        SetButtonOnclick();
        RegisterEvents();
        RefreshUI();
        SaveClear();
        this.GetUtility<UIUtility>().CloseGameUI(gameType);
    }

    public virtual void GetComponents()
    {
        BtnReturn = transform.Find("BtnReturn").GetComponent<Button>();
        BtnShare = transform.Find("BtnShare").GetComponent<Button>();
        BtnRetry = transform.Find("BtnRetry").GetComponent<Button>();
        TxtReturn = BtnReturn.transform.Find("TxtReturn").GetComponent<TextMeshProUGUI>();
        TxtRetry = BtnRetry.transform.Find("TxtReturn").GetComponent<TextMeshProUGUI>();
        TxtSurvival = transform.Find("TxtSurvival").GetComponent<TextMeshProUGUI>();
        TxtDesc = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("TxtDesc").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    public virtual void SetButtonOnclick()
    {
        BtnReturn?.onClick.AddListener(() =>
        {
            OnBtnReturn();
        });

        BtnShare?.onClick.AddListener(() =>
        {
            Vector3 first = pos_1.position;
            Vector3 firstPos = Camera.main.WorldToScreenPoint(first);
            Vector3 second = pos_2.position;
            Vector3 secondPos = Camera.main.WorldToScreenPoint(second);

            shareManager.ShareScreen(firstPos, secondPos);

            //string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
            //languageStr = languageStr.ToUpper();

            //ShareManager.Instance.ShareScreen(languageStr + gameType.ToString());
        });

        BtnRetry?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().CreateGameUI(gameType);
            OnBtnReturn();
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


        TxtReturn.text = textManager.GetConvertText(returnTxt);
        TxtRetry.text = textManager.GetConvertText(retryTxt);
        TxtSurvival.text = textManager.GetConvertText(survivalTxt);
        TxtDesc.text = "";

        var max = m_Model.totalNum;

        for(int i = checkNums.Count - 1; i >=0; i--)
        {
            if (max >= checkNums[i])
            {
                TxtDesc.text += textManager.GetConvertText("Text_" + gameType.ToString() + "TypeContent" + i);
            }
        }
       
        //TxtDesc.text += "\r\n";

    }

    public virtual void SaveClear()
    {
        //this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
        //this.GetUtility<SaveDataUtility>().SaveLevelEndTag(gameType, m_Model.totalLevelNum);
    }

  
}
