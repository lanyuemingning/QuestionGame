using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class BaseSuvivalEndCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    public RadarChart radarChart, radarContentChart;
    [SerializeField]
    public TextMeshProUGUI TxtReturn, TxtProperty_1, TxtProperty_2, TxtProperty_3, TxtProperty_4, TxtProperty_5;
    public TextMeshProUGUI TxtSurvival, TxtSurvivalDay, TxtDesc, TxtRetry;
    [SerializeField]
    public Button BtnReturn, BtnShare, BtnRetry;
    [SerializeField]
    List<int> starCheck = new List<int>();
    [SerializeField]
    List<GameObject> starList = new List<GameObject>();
    [SerializeField]
    Transform pos_1, pos_2;

    int stars = 0;

    //ViewData
    [SerializeField]
    public string returnTxt = "Text_ReturnBegin";
    public string survivalTxt = "Text_Survival";
    public string retryTxt = "Text_Retry";

    [SerializeField]
    public GameType gameType;

    //Model
    public SurvivalModel m_Model;

    //Instance
    public LevelManager levelManager;
    public TextManager textManager;
    public ShareManager shareManager;

    public RectTransform refreshContent;

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        GetComponents();
        GetInstance();

        m_Model = this.GetModel<SurvivalModel>(); //获取model
        m_Model.gameType = gameType;

        SetButtonOnclick();
        RegisterEvents();
        RefreshUI();
        SaveClear();
        this.GetUtility<UIUtility>().CloseGameUI(gameType);
    }

    public virtual void GetComponents()
    {
        radarChart = transform.Find("ImgRadar").GetComponent<RadarChart>();
        radarContentChart = transform.Find("ImgRadarContent").GetComponent<RadarChart>();
        BtnReturn = transform.Find("BtnReturn").GetComponent<Button>();
        BtnShare = transform.Find("BtnShare").GetComponent<Button>();
        BtnRetry = transform.Find("BtnRetry").GetComponent<Button>();
        TxtReturn = BtnReturn.transform.Find("TxtReturn").GetComponent<TextMeshProUGUI>();
        TxtRetry = BtnRetry.transform.Find("TxtReturn").GetComponent<TextMeshProUGUI>();
        TxtProperty_1 = transform.Find("TxtProperty_1").GetComponent<TextMeshProUGUI>();
        TxtProperty_2 = transform.Find("TxtProperty_2").GetComponent<TextMeshProUGUI>();
        TxtProperty_3 = transform.Find("TxtProperty_3").GetComponent<TextMeshProUGUI>();
        TxtProperty_4 = transform.Find("TxtProperty_4").GetComponent<TextMeshProUGUI>();
        TxtProperty_5 = transform.Find("TxtProperty_5").GetComponent<TextMeshProUGUI>();
        TxtSurvival = transform.Find("TxtSurvival").GetComponent<TextMeshProUGUI>();
        TxtSurvivalDay = transform.Find("TxtSurvivalDay").GetComponent<TextMeshProUGUI>();
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
        radarChart._handlerRadio = new List<float>();
        radarContentChart._handlerRadio = new List<float>();

        if(m_Model.propertyNums == 0 && m_Model.propertyNumDic.Count > 0)
        {
            m_Model.propertyNums = m_Model.propertyNumDic.Count;
        }

        for (int i = 0; i < m_Model.propertyNums; i++)
        {
            radarChart._handlerRadio.Add(m_Model.propertyNumDic[i] / 100);
            radarContentChart._handlerRadio.Add(m_Model.propertyNumDic[i] / 100);
        }

        //radarChart.InitPoint();
        radarChart.InitHandlers();
        radarContentChart.InitHandlers();

        
        TxtProperty_1.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_TypeName1");
        TxtProperty_2.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_TypeName2");
        TxtProperty_3.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_TypeName3");
        TxtProperty_4.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_TypeName4");
        TxtProperty_5.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_TypeName5");

        TxtReturn.text = textManager.GetConvertText(returnTxt);
        TxtRetry.text = textManager.GetConvertText(retryTxt);
        TxtSurvival.text = textManager.GetConvertText(survivalTxt);
        TxtDesc.text = "";

        int tag = 1;
        float max = -1;

        for (int i = 0; i < m_Model.propertyNums; i++)
        {
            if(max < m_Model.propertyNumDic[i])
            {
                tag = i + 1;
                max = m_Model.propertyNumDic[i];
            }

            
        }

        if (max >= 70)
        {
            TxtDesc.text += textManager.GetConvertText("Text_" + gameType.ToString() + "_High_TypeName" + tag);
        }
        else if (max >= 30)
        {
            TxtDesc.text += textManager.GetConvertText("Text_" + gameType.ToString() + "_Middle_TypeName" + tag);
        }
        else
        {
            TxtDesc.text += textManager.GetConvertText("Text_" + gameType.ToString() + "_Low_TypeName" + tag);
        }
        //TxtDesc.text += "\r\n";
        if(gameType == GameType.EQ)
        {
            float num = 0;
            for(int i = 0; i < 5; i++)
            {
                num += m_Model.propertyNumDic[i];
            }
            m_Model.survivalDay = (int)num;
            TxtSurvivalDay.text = num.ToString();

            if(m_Model.survivalDay <= 200)
            {
                TxtDesc.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_Answer1");
            }
            else if(m_Model.survivalDay <= 400)
            {
                TxtDesc.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_Answer2");
            }
            else if (m_Model.survivalDay <= 500)
            {
                TxtDesc.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_Answer3");
            }
            else if (m_Model.survivalDay <= 600)
            {
                TxtDesc.text = textManager.GetConvertText("Text_" + gameType.ToString() + "_Answer4");
            }
        }
        else
        {
            TxtSurvivalDay.text = m_Model.survivalDay.ToString() + textManager.GetConvertText("Text_Days");
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(TxtDesc.rectTransform);
        CheckStar();
    }

    public virtual void SaveClear()
    {
        //this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
        //this.GetUtility<SaveDataUtility>().SaveLevelEndTag(gameType, m_Model.survivalDay);
        //for (int i = 0; i < 5; i++)
        //{
        //    this.GetUtility<SaveDataUtility>().SaveLevelPower(gameType, i, m_Model.propertyNumDic[i]);
        //}
    }

    public virtual void CheckStar()
    {
        for(int i = 0; i < starCheck.Count; i++)
        {
            if (starCheck[i] <= m_Model.survivalDay)
            {
                stars = i + 3;
            }
        }

        for(int i = 0; i < starList.Count; i++)
        {
            if(i >= stars)
            {
                starList[i].SetActive(false);
            }
        }
    }
}
