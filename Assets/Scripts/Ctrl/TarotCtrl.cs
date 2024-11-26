using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TarotCtrl : MonoBehaviour, IController
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    List<Sprite> traillsZH = new List<Sprite>(), traillsEN = new List<Sprite>();
    [SerializeField]
    List<Sprite> animalsZH = new List<Sprite>(), animalsEN = new List<Sprite>();
    [SerializeField]
    List<Sprite> colorsZH = new List<Sprite>(), colorsEN = new List<Sprite>();
    [SerializeField]
    List<Sprite> jobsZH = new List<Sprite>(), jobsEN = new List<Sprite>();
    [SerializeField]
    List<Sprite> friendsZH = new List<Sprite>(), friendsEN = new List<Sprite>();
    [SerializeField]
    List<Sprite> romancesZH = new List<Sprite>(), romancesEN = new List<Sprite>();
    [SerializeField]
    List<Image> images = new List<Image>();
    [SerializeField]
    Button BtnNext;
    [SerializeField]
    TextMeshProUGUI TextNext;
    [SerializeField]
    string NextString = "Text_Next";

    TextManager textManager;
    SelectionModel m_Model;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        m_Model = this.GetModel<SelectionModel>();
        int card = m_Model.level / 5;
        textManager = TextManager.Instance;
        switch (card)
        {
            case 1:
                anim.Play("TarotReverse_" + 1);
                break;
            case 2:
                anim.Play("TarotReverse_" + 2);
                break;
            case 3:
                anim.Play("TarotReverse_" + 3);
                break;
            case 4:
                anim.Play("TarotReverse_" + 4);
                break;
        }

        int last = card - 1;
        List<Sprite> useList = null;
        string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
        languageStr = languageStr.ToUpper();
        switch (m_Model.gameType)
        {
            case GameType.Trail:
                if(languageStr == "EN")
                {
                    useList = traillsEN;
                }
                else
                {
                    useList = traillsZH;
                }
                break;
            case GameType.Animal:
                if (languageStr == "EN")
                {
                    useList = colorsEN;
                }
                else
                {
                    useList = colorsZH;
                }
                break;
            case GameType.Color:
                if (languageStr == "EN")
                {
                    useList = colorsEN;
                }
                else
                {
                    useList = colorsZH;
                }
                break;
            case GameType.Job:
                if (languageStr == "EN")
                {
                    useList = jobsEN;
                }
                else
                {
                    useList = jobsZH;
                }
                break;
            case GameType.Friend:
                if (languageStr == "EN")
                {
                    useList = friendsEN;
                }
                else
                {
                    useList = friendsZH;
                }
                break;
            case GameType.Romance:
                if (languageStr == "EN")
                {
                    useList = romancesEN;
                }
                else
                {
                    useList = romancesZH;
                }
                break;

        }
        for (int i = 0; i < last; i++)
        {
            images[i].sprite = useList[i];
        }
        BtnNext.onClick.AddListener(() =>
        {
            NextQuestion();
        });

        TextNext.text = textManager.GetConvertText(NextString);
    }

    public void ChangeCard(int card)
    {
        List<Sprite> useList = null;
        string languageStr = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
        languageStr = languageStr.ToUpper();

        switch (m_Model.gameType)
        {
            case GameType.Trail:
                if (languageStr == "EN")
                {
                    useList = traillsEN;
                }
                else
                {
                    useList = traillsZH;
                }
                break;
            case GameType.Animal:
                if (languageStr == "EN")
                {
                    useList = colorsEN;
                }
                else
                {
                    useList = colorsZH;
                }
                break;
            case GameType.Color:
                if (languageStr == "EN")
                {
                    useList = colorsEN;
                }
                else
                {
                    useList = colorsZH;
                }
                break;
            case GameType.Job:
                if (languageStr == "EN")
                {
                    useList = jobsEN;
                }
                else
                {
                    useList = jobsZH;
                }
                break;
            case GameType.Friend:
                if (languageStr == "EN")
                {
                    useList = friendsEN;
                }
                else
                {
                    useList = friendsZH;
                }
                break;
            case GameType.Romance:
                if (languageStr == "EN")
                {
                    useList = romancesEN;
                }
                else
                {
                    useList = romancesZH;
                }
                break;

        }
        images[card].sprite = useList[card];
    }

    public void CloseUI()
    {
        
    }

    public void NextQuestion()
    {
        m_Model.waitTarot = false;
        this.GetUtility<UIUtility>().CreateChangeQuestion();
        Destroy(gameObject);
    }
}
