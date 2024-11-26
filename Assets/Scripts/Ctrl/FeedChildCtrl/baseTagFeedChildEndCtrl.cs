using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class BaseTagFeedChildEndCtrl : MonoBehaviour, IController
{
    public List<int> tagList = new List<int>();
    //View
    [SerializeField]
    public Image ImgAnswer;
    [SerializeField]
    public TextMeshProUGUI TxtReturn, TxtRetry, TxtJob, TxtTag, TxtUnlock;
    [SerializeField]
    public Button BtnReturn, BtnShare, BtnRetry, BtnUnlock, BtnLeft, BtnRight;
    [SerializeField]
    Transform pos_1, pos_2;
    [SerializeField]
    public GameObject AllNode;

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

    public int nowTag, TagMax, tagIdx;

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        BtnUnlock = transform.Find("BtnUnlock").GetComponent<Button>();
        TxtUnlock = BtnUnlock.transform.Find("TxtUnlockAll").GetComponent<TextMeshProUGUI>(); AllNode = GameObject.Find("AllNode");
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

        BtnLeft?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowTag--;
            tagIdx--;
            if (nowTag <= 1)
            {
                tagIdx = 1;
            }
            if (tagList.Count == 0)
            {
                if (nowTag <= 1)
                {
                    nowTag = 1;
                }
            }
            else
            {
                nowTag = tagList[tagIdx - 1];
            }   
            
            ShowTag(nowTag);
        });

        BtnRight?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowTag++;
            tagIdx++;

            if (tagIdx >= tagList.Count - 1)
            {
                tagIdx = tagList.Count - 1;
            }

            if (tagList.Count == 0)
            {
                if (nowTag >= TagMax)
                {
                    nowTag = TagMax;
                }
            }
            else
            {
                nowTag = tagList[tagIdx - 1];
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
            ShowTag(nowTag);
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
        tagIdx = tagList.IndexOf(tag) + 1;
        ShowTag(tag);
    }

    public virtual void SaveClear()
    {
        this.GetUtility<SaveDataUtility>().SaveLevel((int)gameType);
    }

    public void ShowTag(int tag)
    {
        nowTag = tag;
        //Debug.Log("tag： " + tag);

        //TagData tagData = levelManager.GetTagData(GameType.Trail, tag);
        if (nowTag == 1 || (tagList.Count > 0 && tagIdx == 1))
        {
            BtnLeft.gameObject.SetActive(false);
        }
        else if (nowTag == 2 || (tagList.Count > 0 && tagIdx == 2))
        {
            BtnLeft.gameObject.SetActive(true);
        }
        else if (nowTag == TagMax - 1 || (tagList.Count > 0 && tagIdx == tagList.Count - 2))
        {
            BtnRight.gameObject.SetActive(true);
        }
        else if (nowTag == TagMax || (tagList.Count > 0 && tagIdx == tagList.Count - 1))
        {
            BtnRight.gameObject.SetActive(false);
        }

        string path = "";
        switch (gameType)
        {
            case GameType.Child:
                path = "childanswer";
                TxtTag.text = textManager.GetConvertText("Text_Child_TypeName" + tag);
                break;
            case GameType.ChildEasy:
                path = "childanswer";
                TxtTag.text = textManager.GetConvertText("Text_Child_TypeName" + tag);
                break;
            case GameType.SuperHero:
                path = "superheroanswer";
                TxtTag.text = textManager.GetConvertText("Text_SuperHero_TypeName" + tag);
                break;
        }
        ImgAnswer.sprite = ResourceManager.Instance.Load<Sprite>(path, (tag) + ""); ;
        TxtReturn.text = textManager.GetConvertText(returnTxt);
        TxtRetry.text = textManager.GetConvertText(retryTxt);
        TxtJob.text = textManager.GetConvertText(jobTxt);
        TxtUnlock.text = textManager.GetConvertText("Text_ShowAllAnswer");
    }
    public virtual void ViewAll()
    {
        AllNode.SetActive(true);
        BtnUnlock.gameObject.SetActive(false);
    }
}
