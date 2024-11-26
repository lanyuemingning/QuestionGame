using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class RomanceSelectionCtrl : BaseTagSelectionCtrl
{
    //View
    [SerializeField]
    TextMeshProUGUI BtnText_2_1, BtnText_2_2, BtnText_3_1, BtnText_3_2, BtnText_3_3, BtnText_4_1, BtnText_4_2, BtnText_4_3, BtnText_4_4;
    [SerializeField]
    TextMeshProUGUI TextPercent_1, TextPercent_2, TextPercent_3, TextPercent_4, TextPercentNum_1, TextPercentNum_2, TextPercentNum_3, TextPercentNum_4;
    [SerializeField]
    TextMeshProUGUI TextPercentNext, TextNext, TextPercentContext, TextPercentContextSmall;
    [SerializeField]
    Button Btn_2_1, Btn_2_2, Btn_3_1, Btn_3_2, Btn_3_3, Btn_4_1, Btn_4_2, Btn_4_3, Btn_4_4;
    [SerializeField]
    GameObject TwoSelection, ThreeSelection, FourSelection;


    override public void Start()
    {
        base.Start(); 
    }
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    override public void SetButtonOnclick()
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
        
        Btn_3_1?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectOne();
        });

        Btn_3_2?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectTwo();
        });
                
        Btn_3_3?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectThree();
        });
        
        Btn_4_1?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectOne();
        });

        Btn_4_2?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectTwo();
        });
        
        Btn_4_3?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectThree();
        });

        Btn_4_4?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            SelectFour();
        });

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
  

        if (m_Model.level == m_Model.totalLevelNum)
        {
            ImgProgress.fillAmount = 1;
            OnAllSelectEnd();
        }
        else
        {
            levelData = levelManager.GetLevelData(gameType, m_Model.level);

            SetTextDesc();
            switch (levelData.SelectionNum)
            {
                case 2:
                    TwoSelection.SetActive(true);
                    ThreeSelection.SetActive(false);
                    FourSelection.SetActive(false);
                    BtnText_2_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
                    BtnText_2_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
                    break;

                case 3:
                    TwoSelection.SetActive(false);
                    ThreeSelection.SetActive(true);
                    FourSelection.SetActive(false);
                    BtnText_3_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
                    BtnText_3_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
                    BtnText_3_3.text = textManager.GetConvertText(levelData.SelectionTxt_3);
                    break;

                case 4:
                    TwoSelection.SetActive(false);
                    ThreeSelection.SetActive(false);
                    FourSelection.SetActive(true);
                    BtnText_4_1.text = textManager.GetConvertText(levelData.SelectionTxt_1);
                    BtnText_4_2.text = textManager.GetConvertText(levelData.SelectionTxt_2);
                    BtnText_4_3.text = textManager.GetConvertText(levelData.SelectionTxt_3);
                    BtnText_4_4.text = textManager.GetConvertText(levelData.SelectionTxt_4);
                    break;
            }
        }
    }

    override public void OnAllSelectEnd()
    {
        base.OnAllSelectEnd();
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

    override public void SetPercentImg()
    {
        base.SetPercentImg();
        switch (levelData.SelectionNum)
        {
            case 2:
                TextPercent_1.text = BtnText_2_1.text;
                TextPercent_2.text = BtnText_2_2.text;
                GoPercent3.SetActive(false);
                GoPercent4.SetActive(false);
                break;

            case 3:
                TextPercent_1.text = BtnText_3_1.text;
                TextPercent_2.text = BtnText_3_2.text;
                TextPercent_3.text = BtnText_3_3.text;
                GoPercent3.SetActive(true);
                GoPercent4.SetActive(false);
                break;

            case 4:
                TextPercent_1.text = BtnText_4_1.text;
                TextPercent_2.text = BtnText_4_2.text;
                TextPercent_3.text = BtnText_4_3.text;
                TextPercent_4.text = BtnText_4_4.text;
                GoPercent3.SetActive(true);
                GoPercent4.SetActive(true);
                break;
        }


        TextPercentNum_1.text = levelData.SelectPercent_1 + "%";
        TextPercentNum_2.text = levelData.SelectPercent_2 + "%";
        TextPercentNum_3.text = levelData.SelectPercent_3 + "%";
        TextPercentNum_4.text = levelData.SelectPercent_4 + "%";
        switch (nowSelect)
        {
            case 1:
                ImagePercent_1.sprite = percentSelect;
                ImagePercent_2.sprite = percentOther;
                ImagePercent_3.sprite = percentOther;
                ImagePercent_4.sprite = percentOther;
                break;
            case 2:
                ImagePercent_1.sprite = percentOther;
                ImagePercent_2.sprite = percentSelect;
                ImagePercent_3.sprite = percentOther;
                ImagePercent_4.sprite = percentOther;
                break;
            case 3:
                ImagePercent_1.sprite = percentOther;
                ImagePercent_2.sprite = percentOther;
                ImagePercent_3.sprite = percentSelect;
                ImagePercent_4.sprite = percentOther;
                break;
            case 4:
                ImagePercent_1.sprite = percentOther;
                ImagePercent_2.sprite = percentOther;
                ImagePercent_3.sprite = percentOther;
                ImagePercent_4.sprite = percentSelect;
                break;
        }
    }
}
