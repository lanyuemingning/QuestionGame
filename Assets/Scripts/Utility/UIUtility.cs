using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class UIUtility : IUtility
{
    private ResLoader mResLoader = ResLoader.Allocate();
    GameObject UICanvas, createUI;
     //Stack<GameObject> UIRoot = new Stack<GameObject>();
    Dictionary<string, GameObject> OpenUIDic = new Dictionary<string, GameObject>();
    public void Start()
    {
        ResKit.Init();
    }
    
    public  void CreateGameUI(GameDefine.GameType gameType, bool isAnswer = false)
    {
        string dicKey = gameType.ToString();
        if (isAnswer)
        {
            dicKey = gameType.ToString() + "Answer";
        }
        
        if (OpenUIDic.ContainsKey(dicKey))
        {
            return;
        }
        GameObject go = null;
        if ( UICanvas == null )
        {
            UICanvas = GameObject.Find("UICanvas");
        }
       
        ResourceManager resourceManager = ResourceManager.Instance;
        switch (gameType)
        {
            case GameDefine.GameType.Trail:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UITrailEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UITrailSelection");
                break;
            case GameDefine.GameType.Animal:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIAnimalEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIAnimalSelection");
                break;
            case GameDefine.GameType.Color:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIColorEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIColorSelection");
                break;
            case GameDefine.GameType.Job:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIJobEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIJobSelection");
                break;
            case GameDefine.GameType.Friend:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIFriendEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIFriendSelection");
                break;
            case GameDefine.GameType.Romance:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIRomanceEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIRomanceSelection");
                break;
            case GameDefine.GameType.Child:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIChildEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIFeedChild");
                break;
            case GameDefine.GameType.ChildEasy:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIChildEasyEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIFeedChildEasy");
                break;
            case GameDefine.GameType.Zombie:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIZombieEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIZombieSurvival");
                break;
            case GameDefine.GameType.Island:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIIslandEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIIslandSurvival");
                break;
            case GameDefine.GameType.War:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIWarEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIWarSurvival");
                break;
            case GameDefine.GameType.SuperHero:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UISuperHeroEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UISuperHero");
                break;
            case GameDefine.GameType.SM:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UISMEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UISMSelection");
                break;
            case GameDefine.GameType.Hentai:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIHentaiEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIHentaiSelection");
                break;
            case GameDefine.GameType.XP:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIXPEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIXP");
                break;
            case GameDefine.GameType.EQ:
                go = isAnswer ? resourceManager.Load<GameObject>("prefab/ui/gameanswer", "UIEQEnd") : resourceManager.Load<GameObject>("prefab/ui/gameui", "UIEQ");
                break;
        }

        if (go != null)
        {
            createUI = GameObject.Instantiate(go, UICanvas.transform);
            OpenUIDic.Add(dicKey, createUI);           
        }
        HideUI("UILevelSelect");
    }

    public void CloseGameUI(GameDefine.GameType gameType)
    {
        string name = gameType.ToString();
        if (OpenUIDic.ContainsKey(name))
        {
            GameObject waitClose = OpenUIDic[name];
            OpenUIDic.Remove(name);
            GameObject.Destroy(waitClose);
        }
    }

    public void CloseGameAnswerUI(GameDefine.GameType gameType)
    {
        string name = gameType.ToString() + "Answer";
        if (OpenUIDic.ContainsKey(name))
        {
            GameObject waitClose = OpenUIDic[name];
            OpenUIDic.Remove(name);
            GameObject.Destroy(waitClose);
        }
        ShowUI("UILevelSelect");
    }

    public void OpenUI(string name)
    {
        if (UICanvas == null)
        {
            UICanvas = GameObject.Find("UICanvas");
        }

        GameObject go = ResourceManager.Instance.Load<GameObject>("prefab/ui/others" ,name);
        

     
        if (OpenUIDic.Count == 0)
        {
            GameObject createGo = GameObject.Instantiate(go, UICanvas.transform);
            //UIRoot.Push(createGo);
            OpenUIDic.Add(name, createGo);
        }
        else
        {
            if(OpenUIDic.ContainsKey(name))
            {
                Debug.LogError("已打开相同界面:" + name);
            }
            else
            {
                //GameObject root = UIRoot.Peek();
                GameObject createGo = GameObject.Instantiate(go, UICanvas.transform);
               
                //UIRoot.Push(createGo);
                OpenUIDic.Add(name, createGo);
            }

        }
    }

    public void ShowUI(string name)
    {
        if (OpenUIDic.Count > 0)
        {
            if (OpenUIDic.ContainsKey(name))
            {
                GameObject ShowGo = OpenUIDic[name];
                ShowGo.SetActive(true);
                return;
            }
        }
        
        OpenUI(name);
    }

    public void HideUI(string name)
    {
        if (OpenUIDic.Count > 0)
        {
            if (OpenUIDic.ContainsKey(name))
            {
                GameObject ShowGo = OpenUIDic[name];
                ShowGo.SetActive(false);
            }
        }
    }

    public void CloseUI(string name)
    {
        if (OpenUIDic.Count > 0 )
        {
            if (OpenUIDic.ContainsKey(name))
            {
                GameObject waitCloseGo = OpenUIDic[name];
                OpenUIDic.Remove(name);
                GameObject.Destroy(waitCloseGo);

                //GameObject closeTarget = OpenUIList[name];
                //do
                //{
                //    GameObject waitCloseGo = UIRoot.Pop();
                //    string key = null;
                //    foreach (var pair in OpenUIList)
                //    {
                //        if (pair.Value == waitCloseGo)
                //        {
                //            key = pair.Key;
                //            break;
                //        }
                //    }

                //    OpenUIList.Remove(key);
                //    GameObject.Destroy(waitCloseGo);
                //}
                //while (!OpenUIList.ContainsKey(name));
            }
        }
    }

    public void CreateChangeQuestion()
    {
       GameObject go = ResourceManager.Instance.Load<GameObject>("prefab/ui/others", "UIChangeQuestion");
       GameObject.Instantiate(go, UICanvas.transform);
    }

    public void CreateChangeTarot()
    {
       GameObject go = ResourceManager.Instance.Load<GameObject>("prefab/ui/others", "UITarot");
        GameObject create = GameObject.Instantiate(go, UICanvas.transform);
        create.transform.position = new Vector3(0, 0, 0);
    }
}
