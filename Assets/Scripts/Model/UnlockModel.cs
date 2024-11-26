using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class UnlockModel: AbstractModel
{
    public int unlockProgress = 0, unlockMax = 2;

    public int nowLevel;

    protected override void OnInit()
    {
    }

}
