using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMPLink : MonoBehaviour, IPointerClickHandler, ICanGetUtility
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private TextMeshProUGUI m_TextMeshPro;

    void Awake()
    {
        m_TextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, eventData.pressEventCamera);
        TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_TextMeshPro.rectTransform, eventData.position, eventData.pressEventCamera, out var worldPointInRectangle);
        switch (linkInfo.GetLinkID())
        {
            case "privacy":
                this.GetUtility<UIUtility>().ShowUI("UIPrivacy");
                break;
            case "service":
                Debug.Log("服务条款");
                this.GetUtility<UIUtility>().ShowUI("UIService");
                break;
        }
    }
}
