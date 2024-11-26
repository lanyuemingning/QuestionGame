using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour, IController
{
    public TextMeshProUGUI text;
    public string useStr;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    virtual public void RegisterEvents()
    {
        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            SetText();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    private void Start()
    {
        RegisterEvents();
        SetText();
    }

    public void SetText()
    {
        text.text = TextManager.Instance.GetConvertText(useStr);
    }
}
