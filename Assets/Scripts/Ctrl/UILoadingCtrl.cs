using DG.Tweening;
using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UILoadingCtrl: MonoBehaviour, IController
{
    // Unity Android иообнд
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

    public Image ImgIcon, ImgLoading;

    public TextMeshProUGUI text;

    public static UILoadingCtrl Instance;

    public Sprite EnIcon, ZHIcon;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string languageStr = PlayerPrefs.GetString("g_Language", "-1");
        if (languageStr == "-1")
        {
            languageStr = GetSystemLanguage();
        }
        ImgIcon.sprite = EnIcon;

        switch (languageStr)
        {
            case "zh":
                ImgIcon.sprite = ZHIcon;
                break;
        }
    }

    public void OnEnable()
    {
        PlayAnim();
    }

    public void PlayAnim()
    {
        text.text = "";
        text.DOText("Loading...", 2f).SetEase(Ease.Linear)
          .OnComplete(() =>
          {
              if (gameObject.activeSelf)
              {
                  PlayAnim();
              }
          });
    }

    public string GetSystemLanguage()
    {
        string systemLanguage;
        Debug.Log("Application.platform " + Application.platform);

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaObject locale = UnityContext.Call<AndroidJavaObject>("getResources").Call<AndroidJavaObject>("getConfiguration").Get<AndroidJavaObject>("locale");
            systemLanguage = locale.Call<string>("getLanguage");
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
