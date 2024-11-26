using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class XPEndCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    public Image ImgAnswer;
    [SerializeField]
    public TextMeshProUGUI TxtReturn, TxtRetry, TxtJob, TxtTag, TxtUnlock;
    [SerializeField]
    public Button BtnReturn, BtnShare, BtnRetry, BtnUnlock;
    [SerializeField]
    Transform pos_1, pos_2;
    [SerializeField]
    public GameObject AllNode;
    [SerializeField]
    List<GameObject> XPItems;

    Dictionary<int, bool> hasTag = new Dictionary<int, bool>();
    //ViewData
    [SerializeField]
    public string returnTxt = "Text_ReturnBegin";
    public string retryTxt = "Text_Retry";
    public string jobTxt = "Text_ChildJob";

    [SerializeField]
    public GameType gameType;

    //Model
    public SelectionModel m_Model;

    //Instance
    public LevelManager levelManager;
    public TextManager textManager;
    public ShareManager shareManager;

    public int nowTag, TagMax;

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        BtnUnlock = transform.Find("BtnUnlock").GetComponent<Button>();
        TxtUnlock = BtnUnlock.transform.Find("TxtUnlockAll").GetComponent<TextMeshProUGUI>(); AllNode = GameObject.Find("AllNode");
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
            OnBtnReturn();
        });

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
        });

        BtnRetry?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().CreateGameUI(gameType);
            OnBtnReturn();
        });

        BtnUnlock?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = ViewAll;
            TopOnADManager.Instance.ShowRewardAd();
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
            ShowTag();
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
        var tag = this.GetUtility<SaveDataUtility>().GetLevelEndTag(gameType);
        if(tag == 0)
        {
            for(int i = 0; i < XPItems.Count; i++)
            {
                XPItems[i].SetActive(i == 3);
            }
        }
        else
        {
            for(int i = 0; i < XPItems.Count; i++)
            {
                int check = (int)(tag / (Mathf.Pow(10, i))) % 10;
                XPItems[i].SetActive(false);

                if (check > 0)
                {
                    XPItems[i].SetActive(true);
                } 
            }
        }
        //this.GetUtility<SaveDataUtility>().SaveLevelEndTag(gameType, tag);
        ShowTag();
        //ShowTag(tag);
    }

    public virtual void SaveClear()
    {
    }

    public void ShowTag()
    {
        //nowTag = tag;
        //Debug.Log("tag： " + tag);

        //TagData tagData = levelManager.GetTagData(GameType.Trail, tag);
      

        string path = "";
        TxtReturn.text = TextManager.Instance.GetConvertText(returnTxt);
        TxtRetry.text = TextManager.Instance.GetConvertText(retryTxt);
        TxtTag.text = TextManager.Instance.GetConvertText("Text_XP_AnswerTitle");
    }
    public virtual void ViewAll()
    {
        AllNode.SetActive(true);
        BtnUnlock.gameObject.SetActive(false);
    }
}
