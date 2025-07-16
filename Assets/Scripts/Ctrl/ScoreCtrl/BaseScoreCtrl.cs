using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;

public class BaseScoreCtrl: MonoBehaviour, IController
{
    //View
    [SerializeField]
    public Image ImgShow, ImgProgress, head;
    [SerializeField]
    TextMeshProUGUI TextDesc, BtnText_2_1, BtnText_2_2, BtnText_3_1, BtnText_3_2, BtnText_3_3, BtnText_4_1, BtnText_4_2, BtnText_4_3, BtnText_4_4, TxtProgress;
    TextMeshProUGUI TxtReselect, TxtNext, TextAnswerContext, TextAnswerContextSmall, TxtAnswerReturn, TxtShowAnswer;
    [SerializeField]
    Button Btn_2_1, Btn_2_2, BtnReturn, BtnSet, BtnNext, BtnReselect, BtnAnswerReturn, BtnShowAnswer;
    [SerializeField]
    GameObject TwoSelection, NextPanel, AnswerPanel, OtherThings;
    [SerializeField]
    bool usePicture = false;
    [SerializeField]
    Image Btn1, Btn2;

    //Model
    [NonSerialized]
    public ScoreModel m_Model;

    //Instance
    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public TextManager textManager;
    [HideInInspector]
    public TopOnADManager adManager;
    [HideInInspector]
    public AnalyticsManager analyticsManager;

    //ViewData
    public int nowSelect = 0;
    public LevelData levelData;
    [SerializeField]
    public int noADLevel = 5;
    [SerializeField]
    public GameType gameType;
    public string ImgShowPath = "Sprite/Child/ChildShow/";
    public string nextStr = "Text_PecentNext", reselectStr = "Text_ReSelect";
    public int propertyNums = 5;
    public bool isFirst = true;

    public Dictionary<int, bool> showADDic = new Dictionary<int, bool>()
    {
        [6] = true,
        [10] = true,
        [14] = true,
        [18] = true,
    };

    virtual public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    virtual public void Start()
    {
        GetCoponent();
        GetModel();
        GetInstance();
        ClearADRewardDelegate();
        RegisterEvents();
        SetModelType();
        SetText();
        SetButtonOnclick();
        RefreshUI();
        if (ImgProgress != null)
        {
            ImgProgress.fillAmount = 0.2f;
        }
    }

    /// <summary>
    /// 获取Coponent
    /// </summary>
    virtual public void GetCoponent()
    {
        Transform ot = transform.Find("OtherThings");
        OtherThings = ot.gameObject;
        TextDesc = ot.Find("ImgQuestionTxt").Find("TxtDesc").GetComponent<TextMeshProUGUI>();
        TxtProgress = ot.Find("TxtProgress").GetComponent<TextMeshProUGUI>();
        ImgShow = ot.Find("ImgShow").GetComponent<Image>();
        ImgProgress = ot.Find("ImgProgressBg").Find("ImgProgress").GetComponent<Image>();
        BtnReturn = transform.Find("BtnReturn").GetComponent<Button>();
        BtnSet = transform.Find("BtnSet").GetComponent<Button>();
        Transform nextPanel  = transform.Find("NextPanel");
        NextPanel = nextPanel.gameObject;
        head = nextPanel.Find("head").GetComponent<Image>();
        BtnNext = nextPanel.Find("BtnNext").GetComponent<Button>();
        BtnReselect = nextPanel.Find("BtnReselect").GetComponent<Button>();


        TwoSelection = transform.Find("TwoSelection").gameObject;

        Btn_2_1 = TwoSelection.transform.Find("Btn_1").GetComponent<Button>();
        Btn_2_2 = TwoSelection.transform.Find("Btn_2").GetComponent<Button>();
        if(usePicture)
        {
            Btn1 = TwoSelection.transform.Find("Btn1").GetComponent<Image>();
            Btn2 = TwoSelection.transform.Find("Btn2").GetComponent<Image>();
        }
        BtnText_2_1 = Btn_2_1.transform.Find("BtnText_1").GetComponent<TextMeshProUGUI>();
        BtnText_2_2 = Btn_2_2.transform.Find("BtnText_2").GetComponent<TextMeshProUGUI>();;
        TxtReselect = BtnReselect.transform.Find("TextReselect").GetComponent<TextMeshProUGUI>();
        TxtNext = BtnNext.transform.Find("TextNext").GetComponent<TextMeshProUGUI>();

        Transform asPanel = transform.Find("AnswerPanel");
        AnswerPanel = asPanel.gameObject;
        TextAnswerContext = asPanel.Find("ImgPercentContext/TextAnswerContext").GetComponent<TextMeshProUGUI>();
        BtnAnswerReturn = asPanel.Find("BtnAnswerReturn").GetComponent<Button>();
        BtnShowAnswer = asPanel.Find("BtnShowAnswer").GetComponent<Button>();
        TextAnswerContextSmall = asPanel.Find("ImgPercentContext/TextAnswerContextSmall").GetComponent<TextMeshProUGUI>();
        TxtAnswerReturn = BtnAnswerReturn.transform.Find("TxtAnswerReturn").GetComponent<TextMeshProUGUI>();
        TxtShowAnswer = BtnShowAnswer.transform.Find("TxtShowAnswer").GetComponent<TextMeshProUGUI>();
        AnswerPanel.SetActive(false);
    }

    /// <summary>
    /// 获取model
    /// </summary>
    virtual public void GetModel()
    {
        m_Model = this.GetModel<ScoreModel>(); //获取model
        string childClassName = this.GetType().Name;
        levelManager = LevelManager.Instance;
        m_Model.level = levelManager.Getlevel(childClassName);
        

    }

    virtual public void SetModelType()
    {
        m_Model.ClearModel(propertyNums);
        m_Model.gameType = gameType;
        m_Model.totalLevelNum = levelManager.GetSelectionLevelTotalNum(gameType);
    }

    virtual public void ClearADRewardDelegate()
    {
        adManager.rewardAction = null;
    }
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    virtual public void SetButtonOnclick()
    {
        Btn_2_1?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectOne();
        });

        Btn_2_2?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectTwo();
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UIReturnTip");
        });

        BtnSet?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UISet");
        });

        BtnNext?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            OnBtnNext();
        });

        BtnReselect?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            OnBtnReselect();
        });

        BtnAnswerReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            ReturnToLevelList();
        });

        BtnShowAnswer?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            adManager.rewardAction = ShowAnswerUI;
            adManager.ShowRewardAd();
        });
    }
    /// <summary>
    /// 设置文本
    /// </summary>
    virtual public void SetText()
    {
        TextAnswerContext.text = textManager.GetConvertText("Text_ViewAnswer");
        TextAnswerContextSmall.text = textManager.GetConvertText("Text_PercentContextSmall");
        TxtAnswerReturn.text = textManager.GetConvertText("Text_Return");
        TxtShowAnswer.text = textManager.GetConvertText("Text_ShowPercent");
    }
    /// <summary>
    /// 绑定事件
    /// </summary>
    virtual public void RegisterEvents()
    {
        
        this.RegisterEvent<SelectSuccessEvent>(e =>
        {
            m_Model.level++;
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<ShowNextEvent>(e =>
        {
            ShowNextPanel();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI(true);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<CloseGameUIEvent>(e =>
        {
            ReturnToLevelList();
            levelManager.SaveData(this.GetType().Name, 0);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<CloseGameUISaveEvent>(e =>
        {
            ReturnToLevelList();
            levelManager.SaveData(this.GetType().Name, m_Model.level);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    virtual public void UnRegisterEvents()
    {
    }

    virtual public void GetInstance()
    {
        levelManager = LevelManager.Instance;
        textManager = TextManager.Instance;
        adManager = TopOnADManager.Instance;
        analyticsManager = AnalyticsManager.Instance;
    }

    virtual async public void RefreshUI(bool textRefresh = false)
    {
        TopOnADManager.Instance.RemoveBannerAd();
        bool forceAD = false;

        TxtReselect.text = textManager.GetConvertText(reselectStr);
        TxtNext.text = textManager.GetConvertText(nextStr);
        if (textRefresh)
        {

            
        }
        else
        {
            if (!isFirst)
            {
                this.GetUtility<UIUtility>().CreateChangeQuestion();
                await Task.Run(async () =>
                {

                    Thread.Sleep(600);
                });
            }
            else
            {
                isFirst = false;
            }

            NextPanel.SetActive(false);
        }

        showADDic.TryGetValue(m_Model.level, out forceAD);
        if (forceAD)
        {
            adManager.ShowInterstitialAd();
        }

        if (m_Model.level == 3)
        {
            StartCoroutine(CallReviewManager.Instance.StartReview());
        }

        if (m_Model.level == m_Model.totalLevelNum)
        {
            ImgProgress.fillAmount = 1;
            OnAllSelectEnd();
        }
        else
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "enterQuestion", gameType.ToString() },
                { "enterQuestionNum", m_Model.level + 1 },
            };
            analyticsManager.SendServerEvent("enterQuestion", parameters);

            levelData = levelManager.GetLevelData(gameType, m_Model.level);
            m_Model.levelData = levelData;
            //TextDesc.text = textManager.GetConvertText(levelData.Question);
            TextDesc.text = "";
            TextDesc.DOText(textManager.GetConvertText(levelData.Question), 2f).SetEase(DG.Tweening.Ease.Linear);

            TwoSelection.SetActive(true);
            if(usePicture)
            {
                Btn1.sprite = ResourceManager.Instance.Load<Sprite>(ImgShowPath, (m_Model.level + 1) + ".1");
                Btn2.sprite = ResourceManager.Instance.Load<Sprite>(ImgShowPath, (m_Model.level + 1) + ".2");
                 BtnText_2_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
                BtnText_2_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
            }
            else
            {
                BtnText_2_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
                BtnText_2_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
            }
            TxtProgress.text = (m_Model.level + 1) + "/" + m_Model.totalLevelNum;

            float ans = m_Model.level % 5;

            ImgProgress.fillAmount = (ans + 1) / 5;

            ImgShow.sprite = ResourceManager.Instance.Load<Sprite>(ImgShowPath, (m_Model.level + 1) + "");
            head.sprite = ImgShow.sprite;
        }

        SetText();
    }

    virtual public void OnAllSelectEnd()
    {
        AnswerPanel.SetActive(true);
        TwoSelection.SetActive(false);
        NextPanel.SetActive(false);
        OtherThings.SetActive(false);
    }
        
    virtual public void SetPercentPanel()
    {
        adManager.ShowRewardAd();
        //ShowPercent();

    }

    virtual public void SelectOne()
    {
        nowSelect = 1;
        this.SendCommand<ScoreSelectCommand>();
    }

    virtual public void SelectTwo()
    {
        nowSelect = 2;
        this.SendCommand<ScoreSelectCommand>();
    }

    virtual public void ShowNextPanel()
    {
        NextPanel.SetActive(true);
        TopOnADManager.Instance.ShowBannerAd();
    }

    virtual public void OnBtnNext()
    {
        ShowNextScoreCommand command = new ShowNextScoreCommand();
        command.select = nowSelect;
        this.SendCommand<ShowNextScoreCommand>(command);
    }

    virtual public void OnBtnReselect()
    {
        RefreshUI();
    }
    virtual public void ShowAnswerUI()
    {
        UnRegisterEvents();

        this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
        this.GetUtility<SaveDataUtility>().SaveLevelEndTag(gameType, m_Model.totalNum);

        Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "enterQuestion", gameType.ToString() },
            };
        analyticsManager.SendServerEvent("completeQuestion", parameters);
        this.GetUtility<UIUtility>().CreateGameUI(gameType, true);
    }

    virtual public void ReturnToLevelList()
    {
        this.GetUtility<UIUtility>().CloseGameUI(gameType);
        this.GetUtility<UIUtility>().ShowUI("UILevelSelect");
    }
}
