using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SelectionModel : AbstractModel
{
    /// <summary>
    /// 当前关卡
    /// </summary>
    public int level = 0;

    /// <summary>
    /// 总关卡数
    /// </summary>
    public int totalLevelNum = 0;

    /// <summary>
    /// 选择的标签个数
    /// </summary>
    public Dictionary<int, int> tabCount = new Dictionary<int, int>();

    /// <summary>
    /// 游戏类型
    /// </summary>
    public GameDefine.GameType gameType;

    /// <summary>
    /// 强行赋值结算Tag
    /// </summary>
    public int mostTag = -1;

    /// <summary>
    /// 等待塔罗牌
    /// </summary>
    public bool waitTarot = false;

    /// <summary>
    /// 连续观看数
    /// </summary>
    public int unlockAllNum = 0;

    public int unlickAllMax = 2;

    public bool showAll = false;

    protected override void OnInit()
    {
    }

    public int GetMostTag()
    {
        int max = -1;
        int tag = -1;
        foreach (var item in tabCount)
        {
            //Debug.Log("Tag_Most" + item.Value + " " + item.Key);
            if (item.Value > max)
            {
                max = item.Value;
                tag = item.Key;
            }
        }
        if (mostTag > 0)
        {
            tag = mostTag;
        }

        return tag;
    }

    
    
}
