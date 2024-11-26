using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCommond : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<LevelClearEvent>();
    }
}
