using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    public override void SceneEnter()
    {
        UIManager.Instance.GetUI<UIResult>();
        UIManager.Instance.GetUI<UISelectItem>();
        PlayerItemManager.Instance.ClearItems();
    }

    public override void SceneExit()
    {
    }

    public override void SceneLoad()
    {
    }
}
