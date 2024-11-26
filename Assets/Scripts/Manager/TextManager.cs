using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;

[MonoSingletonPath("[Text]/TextManager")]
public class TextManager : MonoSingleton<TextManager>, ICanGetUtility, ICanSendEvent
{
    TextAsset text_ZH, text_JA, text_EN, text_KO;

    Dictionary<string, string> textDic = new Dictionary<string, string>();

    private void Awake()
    {
        text_ZH = Resources.Load<TextAsset>("Text/Text_ZH");
        text_JA = Resources.Load<TextAsset>("Text/Text_JA");
        text_KO = Resources.Load<TextAsset>("Text/Text_KO");
        text_EN = Resources.Load<TextAsset>("Text/Text_EN");
    }

    public void ReadTextCfg(string languageType)
    {
        textDic.Clear();

        TextAsset textAsset = text_ZH;
        switch (languageType)
        {
            case "zh":
                textAsset = text_ZH;
                break;
            case "ja":
                textAsset = text_JA;
                break;
            case "en":
                textAsset = text_EN;
                break;
            case "ko":
                textAsset = text_KO;
                break;
        }


        string[] textList = textAsset.text.Split("\n");

        foreach (var item in textList)
        {
            int index = item.IndexOf("=");
            string front = item.Substring(0, index - 1);

            string showStr = item.Substring(index + 2, item.Length - index - 2);
            //for(int i = 1; i < textPair.Length; i++)
            //{
            //    showStr += textPair[i];
            //}
            //Debug.Log(front + " " + showStr);

            textDic.Add(front.Trim(), showStr);
        }
    }

    public string GetConvertText(string txt)
    {
        string returnTxt;
        //Debug.Log("转换 " + txt);
        return (textDic.TryGetValue(txt, out returnTxt) ? returnTxt : txt);
    }

    public void ChangeLanguege(GameDefine.LanguageType languageType)
    {
        this.GetUtility<SaveDataUtility>().SaveSelectLanguage(languageType);

        ReadTextCfg(languageType.ToString());

        this.SendEvent<RefreshUITextEvent>();
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

     void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            ChangeLanguege(GameDefine.LanguageType.ja);
        }
        else if(Input.GetKeyUp(KeyCode.J))
        {
            ChangeLanguege(GameDefine.LanguageType.zh);
        }
        
    }
}
