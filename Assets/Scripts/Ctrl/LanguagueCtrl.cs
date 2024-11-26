using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguagueCtrl : MonoBehaviour, IController
{
    [SerializeField]
    Button BtnZhTw, BtnEn, BtnJp, BtnKo;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    

   
}
