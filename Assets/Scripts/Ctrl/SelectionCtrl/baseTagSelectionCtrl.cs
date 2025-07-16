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

public class BaseTagSelectionCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    public Image ImgShow, ImgProgress, ImgGem;
    [SerializeField]
    public Image ImagePercent_1, ImagePercent_2, ImagePercent_3, ImagePercent_4;
    [SerializeField]
    public Sprite percentSelect, percentOther;
    [SerializeField]
    public GameObject SelectionPanel, ADPanel, PercentPanel, OtherThing, AnswerPanel;
    [SerializeField]
    public Button BtnPercentNext, BtnADNext, BtnShowPercent, BtnSet, BtnNoAD, BtnReturn, BtnUnlockAll, BtnAnswerReturn, BtnShowAnswer;
    [SerializeField]
    public TextMeshProUGUI textDesc, TxtProgress, TxtUnlockAll, TextShowPercent, TextAnswerContext, TextAnswerContextSmall;
    public TextMeshProUGUI TxtAnswerReturn, TxtShowAnswer;
    [SerializeField]
    public List<Sprite> gems;
    [SerializeField]
    public GameObject GoPercent1, GoPercent2, GoPercent3, GoPercent4;

    //Model
    [NonSerialized]
    public SelectionModel m_Model;

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
    // 存放虚假的百分比信息
    public LevelData levelData;
    [SerializeField]
    public string percentNextText = "Text_PecentNext", 
        nextText = "Text_Next", 
        showPercentText = "Text_ShowPercent", 
        percentContext = "Text_PercentContext",
        showPercentContextSmall = "Text_PercentContextSmall";
    [SerializeField]
    public int noADLevel = 5;
    [SerializeField]
    public GameType gameType;
    [SerializeField]
    public float fillTime = 2;
    public float questionTime = 2f;
    public string ImgShowPath = "";

    public Dictionary<int, bool> showADDic = new Dictionary<int, bool>()
    {
        [6] = true,
        [10] = true,
        [14] = true,
    };

    public Dictionary<int, bool> showRewardADDic = new Dictionary<int, bool>()
    {
        [18] = true,
    };

    virtual public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    virtual public void Start()
    {
        BtnADNext = transform.Find("ADPanel/BtnADNext").GetComponent<Button>();
        BtnShowPercent = transform.Find("ADPanel/BtnShowPercent").GetComponent<Button>();
        BtnUnlockAll = transform.Find("ADPanel/BtnUnlockAll").GetComponent<Button>();
        TxtUnlockAll = BtnUnlockAll.transform.Find("TxtUnlockAll").GetComponent<TextMeshProUGUI>();
        TextShowPercent = transform.Find("ADPanel/BtnShowPercent/TextShowPercent").GetComponent<TextMeshProUGUI>();
        Transform asPanel = transform.Find("AnswerPanel");
        AnswerPanel = asPanel.gameObject;
        TextAnswerContext = asPanel.Find("ImgPercentContext/TextAnswerContext").GetComponent<TextMeshProUGUI>();
        BtnAnswerReturn = asPanel.Find("BtnAnswerReturn").GetComponent<Button>();
        BtnShowAnswer = asPanel.Find("BtnShowAnswer").GetComponent<Button>();
        TextAnswerContextSmall = asPanel.Find("ImgPercentContext/TextAnswerContextSmall").GetComponent<TextMeshProUGUI>();
        TxtAnswerReturn = BtnAnswerReturn.transform.Find("TxtAnswerReturn").GetComponent<TextMeshProUGUI>();
        TxtShowAnswer = BtnShowAnswer.transform.Find("TxtShowAnswer").GetComponent<TextMeshProUGUI>();
        AnswerPanel.SetActive(false);
        ImgShowPath = gameType.ToString();
        ImgShowPath = ImgShowPath.ToLower();

        GetModel();
        GetInstance();
        ClearADRewardDelegate();
        RegisterEvents();
        SetModelType();
        SetText();
        SetButtonOnclick();
        RefreshUI();
        SetPanelOnClick();
        //if (ImgProgress != null)
        //{
        //    ImgProgress.fillAmount = 0;
        //}
        ChangeStep(3);
    }

    /// <summary>
    /// 获取model
    /// </summary>
    virtual public void GetModel()
    {
        m_Model = this.GetModel<SelectionModel>(); //获取model
       
        
        levelManager = LevelManager.Instance;
        string childClassName = this.GetType().Name;
        m_Model.level = levelManager.Getlevel(childClassName);
        m_Model.tabCount = levelManager.GetTapCount(childClassName);
        Debug.Log(m_Model.level);
        //m_Model.level = 0;
        m_Model.gameType = gameType;
        m_Model.showAll = false;
    }
    
    virtual public void SetModelType()
    {
        m_Model.tabCount.Clear();

        m_Model.gameType = gameType;
        m_Model.mostTag = -1;
        m_Model.totalLevelNum = levelManager.GetSelectionLevelTotalNum(gameType);
    }

    virtual public void SetPanelOnClick()
    {
        BtnPercentNext?.onClick.AddListener(() =>
        {
            this.SendCommand<SelectPercentNextCommand>();
        });

        BtnADNext?.onClick.AddListener(() =>
        {
            this.SendCommand<SelectNextCommand>();
        });

        BtnShowPercent?.onClick.AddListener(() =>
        {
            this.SendCommand<SelectPercentCommand>();
        });

        BtnSet?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UISet");
        });

        BtnNoAD?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");

            string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
            languageStr = languageStr.ToUpper();

            ShareManager.Instance.ShareScreen(languageStr + gameType.ToString() + (m_Model.level + 1));
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UIReturnTip");
        });

        BtnAnswerReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            ReturnToLevelList();
        });

        BtnShowAnswer?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            //adManager.rewardAction = ShowAnswerUI;
            //adManager.ShowRewardAd();
            ShowAnswerUI();
        });

        BtnUnlockAll?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            adManager.rewardAction = ViewAll;
            adManager.ShowRewardAd();
        });
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
    }
    /// <summary>
    /// 设置文本
    /// </summary>
    virtual public void SetText()
    {
        TextAnswerContext.text = textManager.GetConvertText("Text_ViewAnswer"); 
        TextAnswerContextSmall.text = textManager.GetConvertText(showPercentContextSmall);
        TxtAnswerReturn.text = textManager.GetConvertText("Text_Return");
        TxtShowAnswer.text = textManager.GetConvertText(showPercentText);
    }
    /// <summary>
    /// 绑定事件
    /// </summary>
    virtual public void RegisterEvents()
    {      
        this.RegisterEvent<CloseGameUIEvent>(e =>
        {
            ReturnToLevelList();
            levelManager.SaveData(this.GetType().Name, 0, m_Model.tabCount);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<CloseGameUISaveEvent>(e =>
        {
            ReturnToLevelList();
            levelManager.SaveData(this.GetType().Name, m_Model.level, m_Model.tabCount);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    virtual public void ReturnToLevelList()
    {
        this.GetUtility<UIUtility>().CloseGameUI(gameType);
        this.GetUtility<UIUtility>().ShowUI("UILevelSelect");
    }

    virtual public void UnRegisterEvents()
    {
        //adManager.rewardAction -= ShowPercent;
    }

    virtual public void GetInstance()
    {
        levelManager = LevelManager.Instance;
        textManager = TextManager.Instance;
        adManager = TopOnADManager.Instance;
        analyticsManager = AnalyticsManager.Instance;
    }

    virtual public async void RefreshUI(SelectSuccessEvent selectEvent)
    {
        TxtUnlockAll.text = textManager.GetConvertText("Text_UnlockAll") + "(" + m_Model.unlockAllNum + "/" + m_Model.unlickAllMax + ")";

        if (selectEvent.step == 3)
        {
            m_Model.waitTarot = false;
            //bool isTarot = m_Model.level >= 5 && m_Model.level % 5 == 0 && m_Model.level < 20;
            if (m_Model.level != 0)
            {
                //if (isTarot)
                //{
                //    TopOnADManager.Instance.RemoveBannerAd();
                //    this.GetUtility<UIUtility>().CreateChangeTarot();
                //    m_Model.waitTarot = true;
                //}
                //else
                //{
                    this.GetUtility<UIUtility>().CreateChangeQuestion();
                //}
            }

            await Task.Run(async () =>
            {
                //if (isTarot)
                //{
                //    do
                //    {
                //        Thread.Yield();
                //    }
                //    while (m_Model.waitTarot);

                //    Thread.Sleep(600);
                //}
                //else
                //{
                    Thread.Sleep(600);
                //}
            });

            RefreshUI(selectEvent.avoidAD);
            ChangeStep(selectEvent.step);

        }
        else if (selectEvent.step == 1)
        {
            if(m_Model.showAll)
            {
                ChangeStep(selectEvent.step);
            }
            else
            {
                ShowADPanel();
            }
        }
        else if (selectEvent.step == 2)
        {
            ChangeStep(selectEvent.step);
            SetPercentPanel();
        }
    }

    virtual async public void RefreshUI(bool avoidAD = false)
    {
        if (m_Model.level == m_Model.totalLevelNum)
        {

        }
        else
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "enterQuestion", gameType.ToString() },
                { "enterQuestionNum", m_Model.level + 1 },
            };
            analyticsManager.SendServerEvent("enterQuestion", parameters);

            TxtProgress.text = (m_Model.level + 1) + "/" + m_Model.totalLevelNum;

            float ans = m_Model.level % 5;

            ImgProgress.fillAmount = (ans + 1) / 5;

            int useGems = (int)((m_Model.level / 5) - 0.1);
            if (useGems < 0)
            {
                useGems = 0;
            }

            ImgGem.sprite = gems[useGems];

            if(ImgShow != null)
            {
                ImgShow.sprite = ResourceManager.Instance.Load<Sprite>(ImgShowPath, (m_Model.level + 1) + "");
                //ImgShow.sprite = await Addressables.LoadAssetAsync<Sprite>(ImgShowPath + (m_Model.level + 1)).Task;
            }
        }
        
        if (!avoidAD)
        {
            bool forceAD = false;
            showADDic.TryGetValue(m_Model.level, out forceAD);
            if (forceAD)
            {
                adManager.ShowInterstitialAd();
            }
            else
            {
                showRewardADDic.TryGetValue(m_Model.level, out forceAD); 
                if (forceAD)
                {
                    adManager.ShowRewardAd();
                }
            }
        }

        if(m_Model.level == 3)
        {
           StartCoroutine(CallReviewManager.Instance.StartReview());
        }
    }

    virtual public void OnAllSelectEnd()
    {
        //AnswerPanel.SetActive(true);
        PercentPanel.SetActive(false);
        OtherThing.SetActive(false);
        ADPanel.SetActive(false);

        ShowAnswerUI();
    }

    virtual public void ShowAnswerUI()
    {
        UnRegisterEvents();

        int tag = m_Model.GetMostTag();
        if(gameType == GameType.Trail)
        {
            int num = 0;
            for(int i = 1; i <= 5; i++)
            {
                if (m_Model.tabCount[i] > m_Model.tabCount[i + 5])
                {
                    num++;
                }
            }
            tag = num;
        }
        this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
        this.GetUtility<SaveDataUtility>().SaveLevelEndTag(gameType, tag);
        Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "enterQuestion", gameType.ToString() },
            };
        analyticsManager.SendServerEvent("completeQuestion", parameters);
        this.GetUtility<UIUtility>().CreateGameUI(gameType, true);
    }

    virtual public void SetPercentPanel()
    {
        adManager.rewardAction = ShowPercent;
        adManager.ShowRewardAd();
        //ShowPercent();

    }

    virtual public void ViewAll()
    {
        m_Model.unlockAllNum++;

        if(m_Model.unlockAllNum >= m_Model.unlickAllMax)
        {
            m_Model.showAll = true;

            m_Model.unlockAllNum = m_Model.unlickAllMax;
            ChangeStep(2);
            ShowPercent();
        }
        TxtUnlockAll.text = textManager.GetConvertText("Text_UnlockAll") + "(" + m_Model.unlockAllNum + "/" + m_Model.unlickAllMax + ")";
    }

    virtual public void ShowPercent()
    {
        SetPercentImg();
        SetPercentFill();
    }

    virtual public void ShowADPanel()
    {
        //if (m_Model.level < noADLevel)
        if (m_Model.level < 9999)
        {
            ChangeStep(2);
            ShowPercent();
        }
        else
        {
            ChangeStep(1);
        }
    }

    virtual public void SelectOne()
    {
        nowSelect = 1;
        this.SendCommand<FirstSelectionCommand>();
    }

    virtual public void SelectTwo()
    {
        nowSelect = 2;
        this.SendCommand<SecondSelectionCommand>();
    }
    virtual public void SelectThree()
    {
        nowSelect = 3;
        this.SendCommand<ThirdSelectionCommand>();
    }
    virtual public void SelectFour()
    {
        nowSelect = 4;
        this.SendCommand<FourthSelectionCommand>();
    }

    virtual public void ChangeStep(int step)
    {
        switch (step)
        {
            case 1:
                SelectionPanel.SetActive(false);
                
                
                    ADPanel.SetActive(true);
                    PercentPanel.SetActive(false);
                    OtherThing.SetActive(false);
                    BtnUnlockAll.gameObject.SetActive(!m_Model.showAll);
                    TopOnADManager.Instance.ShowBannerAd();
                    if (m_Model.showAll)
                    {
                        ChangeStep(2);
                        ShowPercent();
                    }
                
               
                
                break;
            case 2:
                m_Model.unlockAllNum = 0;

                SelectionPanel.SetActive(false);
                ADPanel.SetActive(false);
                PercentPanel.SetActive(true);
                TopOnADManager.Instance.RemoveBannerAd();

                if (GoPercent1 != null)
                {
                    GoPercent1?.SetActive(false);
                }
                if(GoPercent2 != null)
                {
                    GoPercent2?.SetActive(false);
                }
                if(GoPercent3 != null)
                {
                    GoPercent3?.SetActive(false);
                }
                if(GoPercent4 != null)
                {
                    GoPercent4?.SetActive(false);
                }

                OtherThing.SetActive(true);
                break;
            case 3:
                if(m_Model.level < m_Model.totalLevelNum)
                {
                    SelectionPanel.SetActive(true);
                    ADPanel.SetActive(false);
                    PercentPanel.SetActive(false);
                    OtherThing.SetActive(true);
                }
               

                TopOnADManager.Instance.RemoveBannerAd();

                int tag = m_Model.GetMostTag();
                //Debug.Log("tagNow： " + tag);
                break;
        }
    }

    virtual public void SetPercentImg()
    {
        if (GoPercent1 != null)
        {
            GoPercent1?.SetActive(true);
        }
        if (GoPercent2 != null)
        {
            GoPercent2?.SetActive(true);
        }
        if (GoPercent3 != null)
        {
            GoPercent3?.SetActive(true);
        }
        if (GoPercent4 != null)
        {
            GoPercent4?.SetActive(true);
        }
    }

    virtual public void SetPercentFill()
    {


        if (ImagePercent_1 != null)
        {
            ImagePercent_1.DOKill();
            ImagePercent_1.fillAmount = 0;
            ImagePercent_1.DOFillAmount(levelData.SelectPercent_1 / 100, fillTime);
        }
        if (ImagePercent_2 != null)
        {
            ImagePercent_2.DOKill();
            ImagePercent_2.fillAmount = 0;
            ImagePercent_2.DOFillAmount(levelData.SelectPercent_2 / 100, fillTime);
        }
        if (ImagePercent_3 != null)
        {
            ImagePercent_3.DOKill();
            ImagePercent_3.fillAmount = 0;
            ImagePercent_3.DOFillAmount(levelData.SelectPercent_3 / 100, fillTime);
        }
        if(ImagePercent_4 != null)
        {
            ImagePercent_4.DOKill();
            ImagePercent_4.fillAmount = 0;
            ImagePercent_4.DOFillAmount(levelData.SelectPercent_4 / 100, fillTime);
        }
        

    }

    virtual public void SetTextDesc()
    {
        textDesc.DOKill();
        textDesc.text = "";
        float delayTime = 0;
        //if(m_Model.level > 1)
        //{
        //    delayTime = changeTime;
        //}
        //Debug.Log("changeTime " + changeTime);
        textDesc.DOText(textManager.GetConvertText(levelData.Question), questionTime).SetDelay(delayTime).SetEase(DG.Tweening.Ease.Linear);
    }

    virtual public void ShowAnswer()
    {
        ShowAnswerUI();
    }
}
