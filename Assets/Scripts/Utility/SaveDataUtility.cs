using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;
using System.ComponentModel;
using Unity.VisualScripting;

public class SaveDataUtility : IUtility
{
    public void SaveLevel(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearLevel", 0);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearLevel", clearLevel);

    }

    public bool GetLevelClear(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearLevel", 0);
        int checkLevel = (int)Mathf.Pow(2, level - 1);
        int isClear = clearLevel & checkLevel;
        if(isClear == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SaveLevelUnlock(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearLevelUnlock", 65);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearLevelUnlock", clearLevel);

    }

    public bool GetLevelUnlock(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearLevelUnlock", 65);
        int checkLevel = (int)Mathf.Pow(2, level - 1);
        int isClear = clearLevel & checkLevel;
        if(isClear == 0)
        {
            //return false;
            return true;
        }
        else
        {
            return true;
        }
    }
    
    public void ClearSaveLevel()
    {
        PlayerPrefs.SetInt("g_ClearLevel", 0);
    }

    public int GetClearLevelNum()
    {
        int num = 0;
        for(int i = 1; i <= GameConst.totalLeveNum; i++)
        {
            if(GetLevelClear(i))
            {
                num++;
            }
        }

        return num;
    }

    public string GetSelectLanguage()
    {
        string language =  PlayerPrefs.GetString("g_Language", "-1");

        return language;
    }

    public LanguageType GetSelectLanguageType()
    {
        string language = GetSelectLanguage();

        if(language == LanguageType.en.ToString())
        {
            return LanguageType.en;
        }
        else
        {
            return LanguageType.zh;
        }

    }

    public void SaveSelectLanguage(LanguageType languageType)
    {
        string language = languageType.ToString();
     
        PlayerPrefs.SetString("g_Language", language);
    }

    public void SaveSelectLanguage(string language)
    {
        PlayerPrefs.SetString("g_Language", language);
    }

    public void SaveLevelEndTag(GameType gameType, int tag)
    {
        PlayerPrefs.SetInt("g_LevelEnd" + gameType, tag);
    }

    public int GetLevelEndTag(GameType gameType)
    {
        return PlayerPrefs.GetInt("g_LevelEnd" + gameType, -1);
    }

    public void SaveLevelPower(GameType gameType, int power, float num)
    {
        PlayerPrefs.SetFloat("g_LevelPower" + gameType + power, num);
    }

    public float GetLevelPower(GameType gameType, int power)
    {
        return PlayerPrefs.GetFloat("g_LevelPower" + gameType + power, 0);
    }

    public void SaveTrailTag(int tagType, int tagNum)
    {
        PlayerPrefs.SetInt("g_LevelTrailTag" + tagType, tagNum);
    }

    public int GetTrailTag(int tagType)
    {
        return PlayerPrefs.GetInt("g_LevelTrailTag" + tagType, 0);
    }

    public void ClearAllEndTag()
    {
        foreach(GameType gameType in System.Enum.GetValues(typeof(GameType)))
        {
            SaveLevelEndTag(gameType, -1);
        }
    }

    public int GetPrivacyTip()
    {
        //PlayerPrefs.SetInt("g_PrivacyTip", 0);
        return PlayerPrefs.GetInt("g_PrivacyTip", 1);
    }

    public void SetPrivacyTip(int isTip)
    {
        PlayerPrefs.SetInt("g_PrivacyTip", isTip);
    }

    public int GetReviewTip()
    {
        //PlayerPrefs.SetInt("g_PrivacyTip", 0);
        return PlayerPrefs.GetInt("g_ReviewTip", 0);
    }

    public void SetReviewTip(int isTip)
    {
        PlayerPrefs.SetInt("g_ReviewTip", isTip);
    }


}
