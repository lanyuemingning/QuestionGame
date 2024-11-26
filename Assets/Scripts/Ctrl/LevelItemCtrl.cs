using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemCtrl : MonoBehaviour, IController
{
    //View
    [SerializeField]
    Button BtnLevel;
    [SerializeField]
    GameObject ImgLock, ImgFrame, ImgRight, StarList;
    [SerializeField]
    List<GameObject> Stars;
    [SerializeField]
    TextMeshProUGUI TxtTitle;
    [SerializeField]
    bool isReport = false;

    //ViewData
    [SerializeField]
    GameDefine.GameType gameType;
    [SerializeField]
    GameDefine.GameAllType gameAllType;
    [SerializeField]
    string titleText = "";
    [SerializeField]
    List<int> lockList = new List<int>();
    [SerializeField]
    bool isUnlock = false, isComplete = false;

    //Instance
    TextManager textManager;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    // Start is called before the first frame update
    void Start()
    {
        ImgFrame.SetActive(false);
        GetInstance();
        RegisterEvents();
        SetButtonOnclick();
        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    void SetButtonOnclick()
    {
        BtnLevel?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            EnterLevel();
        });
    }

    void RefreshUI(LevelClearEvent args)
    {
        if(isUnlock)
        {
            if (args.level == (int)gameType)
            {
                isComplete = true;
                ImgLock.SetActive(!isUnlock);
                if (lockList.Count > 0)
                {
                    ImgFrame.SetActive(isUnlock);
                }
                ImgRight.SetActive(isUnlock && isComplete);
            }

            if(isReport)
            {
                for (int i = 0; i < lockList.Count; i++)
                {
                    int lockLevel = lockList[i];
                    bool isClear = this.GetUtility<SaveDataUtility>().GetLevelClear(lockLevel);
                    if (!isClear)
                    {
                        isUnlock = false;
                    }
                    Stars[i]?.SetActive(isClear);

                }
            }
           
        }
        else
        {
            if (isReport)
            {
                isUnlock = true;
                for (int i = 0; i < lockList.Count; i++)
                {
                    int lockLevel = lockList[i];
                    bool isClear = this.GetUtility<SaveDataUtility>().GetLevelClear(lockLevel);
                    if (!isClear)
                    {
                        isUnlock = false;
                    }
                    Stars[i]?.SetActive(isClear);

                }
            }
        }
        StarList?.SetActive(lockList.Count > 0);

    }
    void GetInstance()
    {
        textManager = TextManager.Instance;
    }

    void RegisterEvents()
    {
        this.RegisterEvent<LevelClearEvent>(e =>
        {
            RefreshUI(e);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<UnlockLevelEvent>(e =>
        {
            RefreshUI();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void RefreshUI()
    {
        if (isReport)
        {
            isUnlock = true;
            foreach (int lockLevel in lockList)
            {
                if (!this.GetUtility<SaveDataUtility>().GetLevelClear(lockLevel))
                {
                    isUnlock = false;
                }
            }
        }  
        else
        {
            isUnlock = this.GetUtility<SaveDataUtility>().GetLevelUnlock((int)gameType);
#if UNITY_EDITOR
            //isUnlock = true;
#endif
        }


        if (isUnlock)
        {
            isComplete = this.GetUtility<SaveDataUtility>().GetLevelClear((int)gameType);
        }

        ImgLock.SetActive(!isUnlock);
        ImgRight.SetActive(isUnlock && isComplete);

        if(lockList.Count > 0)
        {
            ImgFrame.SetActive(isUnlock);
        }
        else
        {
            titleText = "Text_LevelTitle" + (int)gameType;
        }

        //string titleColorHex = titleColor.ToHexString().Substring(0, 6);
        //string numColorHex = numColor.ToHexString().Substring(0, 6);
        //TxtTitle.text = "<color=#" + titleColorHex + ">" + textManager?.GetConvertText(titleText) + "</color>";
        TxtTitle.text = textManager?.GetConvertText(titleText);

        RefreshUI(new LevelClearEvent());
        //TxtTitle.color =;
        //TxtNum.color = numColor;
    }

    void EnterLevel()
    {
        if (isReport)
        {
            if(isUnlock)
            {
                switch (gameAllType)
                {
                    case GameAllType.Selection:
                        this.GetUtility<UIUtility>().ShowUI("UISelectionAll");
                        break;
                }
            }
            return;
        } 

        if(isUnlock)
        {
            int levelEnd = this.GetUtility<SaveDataUtility>().GetLevelEndTag(gameType);
            //bool showEnd = levelEnd > 0;
            bool showEnd = isComplete;
            if (showEnd)
            {
                switch (gameAllType)
                {
                    case GameAllType.Selection:
                        this.GetModel<SelectionModel>().mostTag = levelEnd;
                        break;
                    case GameAllType.Survival:
                        SurvivalModel model = this.GetModel<SurvivalModel>();
                        model.survivalDay = levelEnd;
                        for (int i = 0; i < 5; i++)
                        {
                            if (model.propertyNumDic.ContainsKey(i))
                            {
                                model.propertyNumDic[i] = this.GetUtility<SaveDataUtility>().GetLevelPower(gameType, i);
                            }
                            else
                            {
                                model.propertyNumDic.Add(i, this.GetUtility<SaveDataUtility>().GetLevelPower(gameType, i));
                            }
                        }
                        break;
                }
            }

            this.GetUtility<UIUtility>().CreateGameUI(gameType, showEnd);
        }
        else
        {
            this.GetModel<UnlockModel>().nowLevel = (int)gameType;
            this.GetUtility<UIUtility>().OpenUI("UIUnlockTip");

        }
      

        //switch (gameType)
        //{
        //    case GameDefine.GameType.Trail:
        //        break;
        //}
    }
}
