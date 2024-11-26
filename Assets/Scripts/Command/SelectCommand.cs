using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一按键点击Command
/// </summary>
public class FirstSelectionCommand : AbstractCommand 
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
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
public class SecondSelectionCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
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
public class ThirdSelectionCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
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
public class FourthSelectionCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
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
/// 选择查看百分比
/// </summary>
public class SelectPercentCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
        if (model.level < model.totalLevelNum)
        {
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 2;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}
/// <summary>
/// 百分比选择下一题
/// </summary>
public class SelectPercentNextCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
        if (model.level < model.totalLevelNum)
        {
            model.level++;
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 3;
            //sendEvent.avoidAD = true;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}

/// <summary>
/// 选择下一题
/// </summary>
public class SelectNextCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
        if (model.level < model.totalLevelNum)
        {
            model.level++;
            SelectSuccessEvent sendEvent = new SelectSuccessEvent();
            sendEvent.step = 3;
            this.SendEvent<SelectSuccessEvent>(sendEvent);

        }
    }
}

public class ReselectCommand : AbstractCommand
{
    public int select;
    protected override void OnExecute()
    {
        SelectionModel model = this.GetModel<SelectionModel>();
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