using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSetCtrl: MonoBehaviour, IController
{
    [SerializeField]
    Button BtnZhTw, BtnEn, BtnJp, BtnKo, BtnReturn;
    [SerializeField]
    TextMeshProUGUI TextLanguage, TextReturn;

    //Instance
    TextManager textManager;

    //ViewData
    [SerializeField]
    string languageStr = "Text_Language";
    [SerializeField]
    string setReturn = "Text_Return";

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        SetButtonOnclick();
        RegisterEvents();
        GetInstance();
        RefreshUI();
    }
    void GetInstance()
    {
        textManager = TextManager.Instance;
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnZhTw?.onClick.AddListener(() =>
        {
            textManager.ChangeLanguege(GameDefine.LanguageType.zh);
        });

        BtnEn?.onClick.AddListener(() =>
        {
            textManager.ChangeLanguege(GameDefine.LanguageType.en);
        });

        BtnJp?.onClick.AddListener(() =>
        {
            textManager.ChangeLanguege(GameDefine.LanguageType.ja);
        });

        BtnKo?.onClick.AddListener(() =>
        {
            textManager.ChangeLanguege(GameDefine.LanguageType.ko);
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().CloseUI("UILanguage");
        });
    }


    /// <summary>
    /// 绑定事件
    /// </summary>
    void RegisterEvents()
    {
        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void RefreshUI()
    {
        TextLanguage.text = textManager.GetConvertText(languageStr);
        TextReturn.text = textManager.GetConvertText(setReturn);
    }
}
