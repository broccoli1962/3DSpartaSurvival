using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResult : UIBase
{
    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 일시 정지 창 업데이트
        UpdateOwnedItemPanel();
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;
    }

    private void UpdateOwnedItemPanel()
    {

    }
}
