using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class TagAddModel : AbstractModel
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
    /// 属性能力数值
    /// </summary>
    public Dictionary<int, int> tabCount = new Dictionary<int, int>();

    /// <summary>
    /// 游戏类型
    /// </summary>
    public GameDefine.GameType gameType;

    public LevelData levelData;

    protected override void OnInit()
    {
    }

    public void ClearModel(int nums)
    {
        tabCount.Clear();
    }
}
