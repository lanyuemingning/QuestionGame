using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一按键点击Command
/// </summary>
public class FirstTagAddCommand : AbstractCommand 
{
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            foreach (int tag in LevelManager.Instance.GetSelectionTags(model.gameType, model.level, 1))
            {
                if (model.tabCount.ContainsKey(tag))
                {
                    model.tabCount[tag]++;
                }
                else
                {
                    model.tabCount.Add(tag, 1);
                }
            }
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 1;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }

    }
}

/// <summary>
/// 第二按键点击Command
/// </summary>
public class SecondTagAddCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            foreach (int tag in LevelManager.Instance.GetSelectionTags(model.gameType, model.level, 2))
            {
                if (model.tabCount.ContainsKey(tag))
                {
                    model.tabCount[tag]++;
                }
                else
                {
                    model.tabCount.Add(tag, 1);
                }
            }
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 1;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}

/// <summary>
/// 第三按键点击Command
/// </summary>
public class ThirdTagAddCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            foreach (int tag in LevelManager.Instance.GetSelectionTags(model.gameType, model.level, 3))
            {
                if (model.tabCount.ContainsKey(tag))
                {
                    model.tabCount[tag]++;
                }
                else
                {
                    model.tabCount.Add(tag, 1);
                }
            }
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 1;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}

/// <summary>
/// 第四按键点击Command
/// </summary>
public class FourthTagAddCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            foreach (int tag in LevelManager.Instance.GetSelectionTags(model.gameType, model.level, 4))
            {
                if (model.tabCount.ContainsKey(tag))
                {
                    model.tabCount[tag]++;
                }
                else
                {
                    model.tabCount.Add(tag, 1);
                }
            }
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 1;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}
/// <summary>
/// 选择下一题
/// </summary>
public class SelectTagAddNextCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            model.level++;
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 3;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}

public class TagAddReselectCommand : AbstractCommand
{
    public int select;
    protected override void OnExecute()
    {
        TagAddModel model = this.GetModel<TagAddModel>();
        if (model.level < model.totalLevelNum)
        {
            foreach (int tag in LevelManager.Instance.GetSelectionTags(model.gameType, model.level, select))
            {
                if (model.tabCount.ContainsKey(tag))
                {
                    model.tabCount[tag]--;
                }
            }
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 3;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}