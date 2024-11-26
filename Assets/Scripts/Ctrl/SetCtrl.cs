using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class SetCtrl : MonoBehaviour, IController
{
    [SerializeField] 
    TextMeshProUGUI TextLanguage, TextSet, TextReturn;
    [SerializeField]
    Button BtnSound, BtnShare, BtnLanguage, BtnReturn;
    [SerializeField]
    Image ImgSound;

    //Instance
    TextManager textManager;
    ShareManager shareManager;

    //ViewData
    [SerializeField]
    Sprite soundsOn, soundsOff;
    [SerializeField]
    string setText = "Text_Set";
    [SerializeField]
    string setReturn = "Text_Return";
    [SerializeField]
    bool isSoundOn = true;

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
        shareManager = ShareManager.Instance;
    }

    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnSound?.onClick.AddListener(() =>
        {
            AudioKit.Settings.IsSoundOn.Value = !AudioKit.Settings.IsSoundOn.Value;
            AudioKit.Settings.IsMusicOn.Value = !AudioKit.Settings.IsMusicOn.Value;
            AudioKit.Settings.IsVoiceOn.Value = !AudioKit.Settings.IsVoiceOn.Value;
            ImgSound.sprite = AudioKit.Settings.IsMusicOn.Value ? soundsOn : soundsOff;
        });

        BtnShare?.onClick.AddListener(() =>
        {
            shareManager.ShareUrl();
        });

        BtnLanguage?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().ShowUI("UILanguage");
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().CloseUI("UISet");
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
        TextSet.text = textManager.GetConvertText(setText);
        TextReturn.text = textManager.GetConvertText(setReturn);
        string language = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
        language = language.ToUpper();
        language = language.Substring(0, 1) + " " + language.Substring(1, 1);
        TextLanguage.text = language;

        ImgSound.sprite = AudioKit.Settings.IsMusicOn.Value ? soundsOn : soundsOff;
    }
}
