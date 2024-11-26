using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintDebug : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI printText;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
       
        Application.logMessageReceived += HandleLog;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void HandleLog(string message, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                message = "<color=#FF0000>" + message + "</color>";
                printText.text += "\r\n" + message;
                break;
            case LogType.Assert:
                message = "<color=#0000ff>" + message + "</color>";
                //printText.text += "\r\n" + message;
                break;
            case LogType.Warning:
                message = "<color=#EEEE00>" + message + "</color>";
                //printText.text += "\r\n" + message;
                break;
            case LogType.Log:
                message = "<color=#FFFFFF>" + message + "</color>";
                printText.text += "\r\n" + message;

                break;
            case LogType.Exception:
                break;
            default:
                break;
        }

    }
}
