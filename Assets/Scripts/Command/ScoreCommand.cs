using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSelectCommand : AbstractCommand
{
    protected override void OnExecute()
    {
       
            ShowNextEvent sendEvent = new ShowNextEvent();
            this.SendEvent<ShowNextEvent>(sendEvent);

    }
}

public class ShowNextScoreCommand : AbstractCommand
{
    public int select;
    protected override void OnExecute()
    {
        ScoreModel model = this.GetModel<ScoreModel>();
        if (model.level < model.totalLevelNum)
        {
            Debug.Log("分数选择 " + select);
            model.AddProperties(LevelManager.Instance.GetSelectionTags(model.gameType, model.level, select));
        }

        SelectSuccessEvent sendEvent = new SelectSuccessEvent();
        this.SendEvent<SelectSuccessEvent>(sendEvent);
    }
}