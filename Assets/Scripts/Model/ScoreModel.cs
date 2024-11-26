using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class ScoreModel : AbstractModel
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
    public int totalNum = 0;

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
        totalNum = 0;
    

    }

    public void AddProperties(List<int> properties)
    {
        totalNum += properties[0];
    }
}
