using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPItemCtrl : MonoBehaviour, IController
{
    public TextMeshProUGUI title, desc;
    public int useTag;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        this.RegisterEvent<RefreshUITextEvent>(e =>
        {
            SetText();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        SetText();
    }

    void SetText()
    {
        title.text = TextManager.Instance.GetConvertText("Text_XP_TypeName" + useTag);
        desc.text = TextManager.Instance.GetConvertText("Text_XP_TypeContent" + useTag);
    }
   
}
