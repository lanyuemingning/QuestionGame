using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelCtrl: MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField] 
    TextMeshProUGUI TxtUnlockAll, TextTitle, TextUnlock, TxtLevelTitle, TxtDesc;
    [SerializeField]
    Button BtnUnlockAll, BtnUnlock, BtnReturn;
    [SerializeField]
    Image ImgSprite;

    //Instance
    TextManager textManager;

    [SerializeField]
    string strTitle = "Text_UnlockLevel";
    [SerializeField]
    bool isSoundOn = true;
    public int levelNum
    {
        get
        {
            return this.GetModel<UnlockModel>().nowLevel;
        }
    }
    [SerializeField]
    List<Sprite> levelImgList = new List<Sprite>();

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
        BtnUnlock?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = Unlock;
            TopOnADManager.Instance.ShowRewardAd();
        });

        BtnReturn?.onClick.AddListener(() =>
        {
            this.GetUtility<UIUtility>().CloseUI("UIUnlockTip");
        });

        BtnUnlockAll?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            TopOnADManager.Instance.rewardAction = UnlockAll;
            TopOnADManager.Instance.ShowRewardAd();
        });
    }
    void UnlockAll()
    {
        UnlockModel model = this.GetModel<UnlockModel>();
        model.unlockProgress++;
        SetUnlockAll();

        if (model.unlockProgress >= model.unlockMax)
        {
            for (int i = 1; i < 12; i++)
            {
                this.GetUtility<SaveDataUtility>().SaveLevelUnlock(i);
            }

            model.unlockProgress = model.unlockMax;
            this.SendEvent<UnlockLevelEvent>();
            this.GetUtility<UIUtility>().CloseUI("UIUnlockTip");
        }

    }

    void Unlock()
    {
        this.GetUtility<SaveDataUtility>().SaveLevelUnlock(levelNum);
        this.SendEvent<UnlockLevelEvent>();
        this.GetUtility<UIUtility>().CloseUI("UIUnlockTip");
    }

    void SetUnlockAll()
    {
        UnlockModel model = this.GetModel<UnlockModel>();
        TxtUnlockAll.text = textManager.GetConvertText("Text_UnlockAll") + "(" + model.unlockProgress + "/" + model.unlockMax + ")";
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
        ImgSprite.sprite = levelImgList[levelNum - 1];
        TextTitle.text = textManager.GetConvertText(strTitle);        
        TxtUnlockAll.text = textManager.GetConvertText("Text_UnlockAll");        
        TextUnlock.text = textManager.GetConvertText("Text_UnlockNowLevel");
        TxtLevelTitle.text = textManager.GetConvertText("Text_LevelTitle" + levelNum);
        TxtDesc.text = textManager.GetConvertText("Text_EarlyUnlock");

        SetUnlockAll();
    }
}
