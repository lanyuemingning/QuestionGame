using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class TrailEndCtrl : MonoBehaviour, IController
{
    public List<TagItemCtrl> tagItems = new List<TagItemCtrl>();
    public TextMeshProUGUI txtTitle, txtTag, txtTagContent, TxtRetry, TxtReturn, txtSubTitle;
    public Image imgContent, imgShape, imgBg;
    public List<Color> colorList = new List<Color>();
    public List<Sprite> sprites = new List<Sprite>();
    public List<Sprite> bgs = new List<Sprite>();
    public GameObject allAnswer, myAnswer;
    public Button BtnReturn, BtnRetry, BtnUnlock;
    int trailEndTag = 0;
    virtual public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    virtual public void SetText()
    {
        txtTitle.text = TextManager.Instance.GetConvertText("Text_Trail_Answer");
        txtTag.text = TextManager.Instance.GetConvertText("Text_Trail_TypeName" + trailEndTag);
        txtTagContent.text = TextManager.Instance.GetConvertText("Text_Trail_TypeDesc" + trailEndTag);
        TxtRetry.text = TextManager.Instance.GetConvertText("Text_Retry");
        TxtReturn.text = TextManager.Instance.GetConvertText("Text_ReturnBegin");
        txtSubTitle.text = TextManager.Instance.GetConvertText("Text_Trail_SubTitle");
        //TextAnswerContextSmall.text = textManager.GetConvertText(showPercentContextSmall);
        //TxtAnswerReturn.text = textManager.GetConvertText("Text_Return");
        //TxtShowAnswer.text = textManager.GetConvertText(showPercentText);
    }

    public virtual void OnBtnReturn()
    {
        this.GetUtility<UIUtility>().CloseGameAnswerUI(GameType.Trail);
    }

    public void RegistEvents()
    {
        BtnRetry?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().CreateGameUI(GameType.Trail);
            OnBtnReturn();
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            OnBtnReturn();
        });

        BtnUnlock?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = ViewAll;
            TopOnADManager.Instance.ShowRewardAd();
        });
    }

    public void ViewAll()
    {
        allAnswer.SetActive(true);
        myAnswer.SetActive(false);
    }

    private void Start()
    {
        Debug.Log("电车结束");
        this.GetUtility<UIUtility>().CloseGameUI(GameType.Trail);
        RegistEvents();
        float tagSum = 0;
        //m_Model.tabCount
        for(int i = 0; i < tagItems.Count; i++)
        {
            var item = tagItems[i];
            
            float tagNum = this.GetUtility<SaveDataUtility>().GetTrailTag(i);
            tagSum += tagNum;
            item.Init(tagNum / 20);

        }

        trailEndTag = this.GetUtility<SaveDataUtility>().GetLevelEndTag(GameType.Trail);
        Debug.Log("电车结束Tag" + trailEndTag + " " + colorList.Count);

        foreach (var item in tagItems)
        {
            item.scrollImg.color = colorList[trailEndTag - 1];
        }
        //imgContent.color = colorList[trailEndTag - 1];
        //txtTag.color = colorList[trailEndTag - 1];
        imgShape.sprite = sprites[trailEndTag - 1];
        imgBg.sprite = bgs[trailEndTag - 1];
        SetText();
    }
}
