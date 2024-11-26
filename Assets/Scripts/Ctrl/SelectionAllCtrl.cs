using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class SelectionAllCtrl: MonoBehaviour, IController
{
    [SerializeField]
    Button BtnReturn, BtnSet;
    [SerializeField]
    List<Image> imageList = new List<Image>();
    [SerializeField]
    List<TextMeshProUGUI> textLists = new List<TextMeshProUGUI>();
    [SerializeField]
    List<Button> buttonLists = new List<Button>();
    [SerializeField]
    TextMeshProUGUI Text_Summary;
    [SerializeField]
    List<GameType> gameTypeList = new List<GameType>();
    [SerializeField]
    TextMeshProUGUI TxtJob, TxtTag;
    int trailEndTag = 0;
    public List<TagItemCtrl> tagItems = new List<TagItemCtrl>();
    public TextMeshProUGUI txtTitle, txtTag, txtTagContent, txtSubTitle;
    public List<Color> colorList = new List<Color>();
    public Image imgContent, imgShape;
    public List<Sprite> sprites = new List<Sprite>();

    //Instance
    TextManager textManager;
    ShareManager shareManager;

    List<int> answers = new List<int>();

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        trailEndTag = this.GetUtility<SaveDataUtility>().GetLevelEndTag(GameType.Trail);
        SetButtonOnclick();
        RegisterEvents();
        GetInstance();
        RefreshUIText();
        SetTrail();
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

        for(int i = 0; i < buttonLists.Count; i++)
        {
            int num = i;

            buttonLists[i]?.onClick.AddListener(() =>
            {
                ShowAnswer(num);
            });
        }

        BtnReturn?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UILevelSelect");
            this.GetUtility<UIUtility>().HideUI("UISelectionAll");

        });

        BtnSet?.onClick.AddListener(() =>
        {
            AudioKit.PlaySound("resources://Sound/btnClick");
            this.GetUtility<UIUtility>().ShowUI("UISet");
            //shareManager.ShareScreen();
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
    void GetInstance()
    {
        textManager = TextManager.Instance;
        shareManager = ShareManager.Instance;
    }

    void RefreshUI()
    {
        SetTrail();

        for (int i = 1; i < 6; i++)
        {
            SetAnswerImg(i);
        }

        RefreshUIText();
    }

    void SetTrail()
    {
        float tagSum = 0;
        //m_Model.tabCount
        for (int i = 0; i < tagItems.Count; i++)
        {
            var item = tagItems[i];

            float tagNum = this.GetUtility<SaveDataUtility>().GetTrailTag(i + 1);
            tagSum += tagNum;
            item.Init(tagNum / 20);

        }
        imgContent.color = colorList[trailEndTag];
        txtTag.color = colorList[trailEndTag];
        imgShape.sprite = sprites[trailEndTag];
        SetTrailText();
    }

    void SetTrailText()
    {
        txtTitle.text = TextManager.Instance.GetConvertText("Text_Trail_Answer");
        txtTag.text = TextManager.Instance.GetConvertText("Text_Trail_TypeName" + trailEndTag);
        txtTagContent.text = TextManager.Instance.GetConvertText("Text_Trail_TypeDesc" + trailEndTag);
        txtSubTitle.text = TextManager.Instance.GetConvertText("Text_Trail_SubTitle");
    }

    void RefreshUIText()
    {
        for (int i = 0; i < textLists.Count; i++)
        {
            textLists[i].text = textManager?.GetConvertText("Text_LevelTitle" + (int)gameTypeList[i]);
        };

        Text_Summary.text = textManager?.GetConvertText("Text_SelectionSummary");
        SetTrailText();
    }

    void SetAnswerImg(int idx)
    {
        GameType gameType = gameTypeList[idx];
        int levelEnd = this.GetUtility<SaveDataUtility>().GetLevelEndTag(gameType);
        answers.Add(levelEnd);

        string answerImgPath = "";
        string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
        languageStr = languageStr.ToUpper();

        switch (gameType)
        {
            case GameType.Trail:
                answerImgPath = "trailanswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Animal:
                answerImgPath = "animalanswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Color:
                answerImgPath = "coloranswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Job:
                answerImgPath = "JobAnswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Friend:
                answerImgPath = "FriendAnswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Romance:
                answerImgPath = "RomanceAnswer/";
                answerImgPath += languageStr;
                //answerImgPath = answerImgPath + levelEnd;
                break;
            case GameType.Child:
                answerImgPath = "ChildAnswer";

                TxtJob.text = TextManager.Instance.GetConvertText("Text_ChildJob");
                TxtTag.text = TextManager.Instance.GetConvertText("Text_Child_TypeName" + levelEnd);
                break;
        }
        answerImgPath = answerImgPath.ToLower();
        imageList[idx].sprite = ResourceManager.Instance.Load<Sprite>(answerImgPath, levelEnd +"");
    }

    void ShowAnswer(int idx)
    {
        GameType gameType = gameTypeList[idx];

        int levelEnd = answers[idx];
        bool showEnd = levelEnd > 0;
        if (showEnd)
        {
            this.GetModel<SelectionModel>().mostTag = levelEnd;
        }

        this.GetUtility<UIUtility>().CreateGameUI(gameType, showEnd);
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    shareManager.ShareScreen();
        //}
    }
}
