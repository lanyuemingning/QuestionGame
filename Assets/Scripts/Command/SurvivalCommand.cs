using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSelectCommand : AbstractCommand
{
    protected override void OnExecute()
    {
       
            ShowNextEvent sendEvent = new ShowNextEvent();
            this.SendEvent<ShowNextEvent>(sendEvent);

    }
}

public class ShowNextCommand : AbstractCommand
{
    public int select;
    protected override void OnExecute()
    {
        SurvivalModel model = this.GetModel<SurvivalModel>();
        if (model.level < model.totalLevelNum)
        {
            Debug.Log("生存选择 " + select);
            model.AddProperties(LevelManager.Instance.GetSelectionTags(model.gameType, model.level, select));
            model.AddSurvival(select);
        }

        SelectSuccessEvent sendEvent = new SelectSuccessEvent();
        this.SendEvent<SelectSuccessEvent>(sendEvent);
    }
}