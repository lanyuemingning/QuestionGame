using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SurvivalModel : AbstractModel
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
    public Dictionary<int, float> propertyNumDic = new Dictionary<int, float>();

    /// <summary>
    /// 游戏类型
    /// </summary>
    public GameDefine.GameType gameType;

    /// <summary>
    /// 生存天数
    /// </summary>
    public int survivalDay = 0;

    public LevelData levelData;

    public int propertyNums;

    protected override void OnInit()
    {
    }

    public void ClearModel(int nums)
    {
        survivalDay = 0;
        propertyNumDic.Clear();
        propertyNums = nums;
        for(int i = 0; i < propertyNums; i++)
        {
            propertyNumDic.Add(i, 0);
        }

    }

    public void AddProperties(List<int> properties)
    {
        for(int i = 0; i < propertyNums; i++)
        {
            propertyNumDic[i] += properties[i];
        }
    }
    
    public void AddSurvival(int selection)
    {
        switch(selection)
        {
            case 1:
                survivalDay += (int)levelData.SelectPercent_1;
                break;
            case 2:
                survivalDay += (int)levelData.SelectPercent_2;
                break;
            case 3:
                survivalDay += (int)levelData.SelectPercent_3;
                break;
            case 4:
                survivalDay += (int)levelData.SelectPercent_4;
                break;

        }
    }
}
