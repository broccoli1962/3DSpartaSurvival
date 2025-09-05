using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : BaseScene
{
    public override void SceneEnter()
    {
        UIManager.Instance.OpenUI<UITitle>();
    }

    public override void SceneExit()
    {
    }

    public override void SceneLoad()
    {
    }
}
