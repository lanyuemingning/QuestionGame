using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageUtility : IUtility
{
    // Unity Android 上下文
    private static AndroidJavaObject _unityContext;

    public static AndroidJavaObject UnityContext
    {
        get
        {

            if (_unityContext == null)
            {
                AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _unityContext = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            return _unityContext;
        }
    }

    /// <summary>
    /// 区分中文的简体繁体
    /// 情况分为2种：
    /// 1、
    ///  String ZH = "zh";
    /// 中文简体
    /// String CN_HANS = "zh-Hans";
    /// 中文繁体
    /// String CN_HANT = "zh-Hant";
    /// 2、
    /// locale.toLanguageTag()
    /// 中国 zh-Hans-CN
    /// 台湾 zh-Hans-TW
    /// 澳门 zh-Hans-MO
    /// 香港 zh-Hans-HK
    /// </summary>
    /// <returns></returns>
    public string GetSystemLanguage()
    {
        string systemLanguage;
        Debug.Log("Application.platform " + Application.platform);

        if (Application.platform == RuntimePlatform.Android)
        {

            Debug.Log("9999999999999");
            AndroidJavaObject locale = UnityContext.Call<AndroidJavaObject>("getResources").Call<AndroidJavaObject>("getConfiguration").Get<AndroidJavaObject>("locale");
            systemLanguage = locale.Call<string>("getLanguage");
            Debug.Log("100000000000000000");

            //Debug.Log(systemLanguage);
            //if (locale.Call<string>("getLanguage").Equals("zh"))
            //{
            //    if (locale.Call<string>("toLanguageTag").Equals("zh-Hans"))
            //    {
            //        systemLanguage = "简体中文 " + locale.Call<string>("toLanguageTag");
            //    }
            //    else if (locale.Call<string>("toLanguageTag").Equals("zh-Hant"))
            //    {
            //        systemLanguage = "繁体中文 " + locale.Call<string>("toLanguageTag");
            //    }
            //    else
            //    { // 第二种简繁中文情况
            //        switch (locale.Call<string>("getCountry"))
            //        {
            //            case "CN":

            //                systemLanguage = "简体中文 " + locale.Call<string>("toLanguageTag");
            //                break;

            //            default:
            //                systemLanguage = "繁体中文 " + locale.Call<string>("toLanguageTag");
            //                break;
            //        }
            //    }
            //}
            //else if (locale.Call<string>("getLanguage").Equals("ko") || locale.Call<string>("getLanguage").StartsWith("ja"))
            //{
            //    systemLanguage = "日文" + locale.Call<string>("toLanguageTag");
            //}
            //else
            //{
            //    systemLanguage = "非中文 " + locale.Call<string>("toLanguageTag");
            //}
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    systemLanguage = "en"; 
                    break;
                case SystemLanguage.Japanese:
                    systemLanguage = "ja";
                    break;
                case SystemLanguage.Chinese:
                    systemLanguage = "zh";
                    break;
                default:
                    systemLanguage = "zh";
                    break;
            }
        }

        return systemLanguage;
    }
}
