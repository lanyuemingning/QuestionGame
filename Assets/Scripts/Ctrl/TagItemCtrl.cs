using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagItemCtrl : MonoBehaviour, IController
{
    public float percent;
    public int tag1, tag2;
    public Scrollbar scrollbar;
    public Image scrollImg;
    public TextMeshProUGUI txtTag1, txtTag2, txtTagContent, txtTagPercent1, txtTagPercent2;
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
    }

    public void Init(float p)
    {
        percent = p;

        if(p > 0.98f)
        {
            scrollbar.size = 0.98f;
        }
        else
        {
            scrollbar.size = p;
        }
        SetText();
    }

    public void SetText()
    {
        txtTag1.text = TextManager.Instance.GetConvertText("Text_Trail_Tag" + tag1);
        txtTag2.text = TextManager.Instance.GetConvertText("Text_Trail_Tag" + tag2);
        txtTagPercent1.text = (1 - percent) * 100 + "%";
        txtTagPercent2.text = percent * 100 + "%";
        if (percent < 0.5f)
        {
            txtTagContent.text = TextManager.Instance.GetConvertText("Text_Trail_TagDesc" + tag1);
        }
        else
        {
            txtTagContent.text = TextManager.Instance.GetConvertText("Text_Trail_TagDesc" + tag2);
        }
    }
}
