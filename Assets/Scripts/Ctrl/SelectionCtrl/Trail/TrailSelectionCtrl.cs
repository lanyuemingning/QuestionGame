using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class TrailSelectionCtrl : BaseTagSelectionCtrl
{
    //View
    [SerializeField]
    TextMeshProUGUI BtnText_1, BtnText_2, TextPercent_1, TextPercent_2, TextPercentNum_1, TextPercentNum_2;
    [SerializeField]
    TextMeshProUGUI TextPercentNext, TextNext, TextPercentContext, TextPercentContextSmall;
    [SerializeField]
    Button Btn_1, Btn_2;
    [SerializeField]
    GameObject ShowNode;
    //ViewData
    GameObject nowShow;
    [SerializeField]
    Animator nowAnim;

    ShareManager shareManager;
    override public void Start()
    {
        base.Start();
        shareManager = ShareManager.Instance;
    }

    override public void SetText()
    {
        base.SetText();
        TextPercentNext.text = textManager.GetConvertText(percentNextText);
        TextNext.text = textManager.GetConvertText(nextText);
        TextShowPercent.text = textManager.GetConvertText(showPercentText);
        TextPercentContext.text = textManager.GetConvertText(percentContext);
        TextPercentContextSmall.text = textManager.GetConvertText(showPercentContextSmall);
    }
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    override public void SetButtonOnclick()
    {
        Btn_1?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowAnim.Play("TrailUp");
            Invoke("SelectOne", 1.5f);
            Btn_1.interactable = false;
            Btn_2.interactable = false;

        });

        Btn_2?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            nowAnim.Play("TrailDown");
            Invoke("SelectTwo", 1.5f);
            Btn_1.interactable = false;
            Btn_2.interactable = false;
        });

        //BtnPercentNext?.onClick.AddListener(() =>
        //{
        //    this.SendCommand<SelectPercentNextCommand>();
        //});

        //BtnADNext?.onClick.AddListener(() =>
        //{
        //    this.SendCommand<SelectNextCommand>();
        //});

        //BtnShowPercent?.onClick.AddListener(() =>
        //{
        //    this.SendCommand<SelectPercentCommand>();
        //});
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    override public void RegisterEvents()
    {
        base.RegisterEvents();
        this.RegisterEvent<SelectSuccessEvent>(e =>
        {
            RefreshUI(e);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI(true);
            SetText();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

    }

    override public void RefreshUI(bool avoidAD = false)
    {
        base.RefreshUI(avoidAD);
  
        //if (!this.GetUtility<SaveDataUtility>().GetLevelClear((int)GameType.Trail))
        //{
        //    BtnReturn?.gameObject.SetActive(false); 
        //}

            //SelectionPanel.SetActive(true);
            //ADPanel.SetActive(false);
            //PercentPanel.SetActive(false);
            //OtherThing.SetActive(true);
        Btn_1.interactable = true;
        Btn_2.interactable = true;

        if (m_Model.level == m_Model.totalLevelNum)
        {
            ImgProgress.fillAmount = 1;
            OnAllSelectEnd();
        }
        else
        {
            if (nowShow)
            {
                nowAnim = null;
                GameObject.Destroy(nowShow);
            }

            levelData = levelManager.GetLevelData(gameType, m_Model.level);

            SetTextDesc();
            BtnText_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
            BtnText_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
          

            //ImgShow.sprite = Resources.Load<Sprite>("Sprite/Trail/TrailShow/" + (m_Model.level + 1));
            GameObject showPrefab = ResourceManager.Instance.Load<GameObject>("uitrailshow",  (m_Model.level + 1).ToString());
            nowShow = GameObject.Instantiate(showPrefab, ShowNode.transform);
            nowShow.transform.localPosition = Vector3.zero;
            nowAnim = nowShow.GetComponent<Animator>();
        }
    }

    override public void OnAllSelectEnd()
    {
        base.OnAllSelectEnd();

        this.GetUtility<SaveDataUtility>().SaveLevel((int)GameType.Trail);
        int tagSum = 0;
        for (int i = 0; i < 5; i++)
        {
            var tagNum = m_Model.tabCount[i + 1];
            tagSum += tagNum;
            this.GetUtility<SaveDataUtility>().SaveTrailTag(i, tagNum);
        }
        int endTag = tagSum / 20;
        endTag = 5 - endTag;
    }

    override public void SetPercentImg()
    {
        base.SetPercentImg();
        TextPercent_1.text = BtnText_1.text;
        TextPercent_2.text = BtnText_2.text;
        TextPercentNum_1.text = levelData.SelectPercent_1 + "%";
        TextPercentNum_2.text = levelData.SelectPercent_2 + "%";
        switch (nowSelect)
        {
            case 1:
                ImagePercent_1.sprite = percentSelect;
                ImagePercent_2.sprite = percentOther;
                break;
            case 2:
                ImagePercent_1.sprite = percentOther;
                ImagePercent_2.sprite = percentSelect;
                break;
        }
    }
}
