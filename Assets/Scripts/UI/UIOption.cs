using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOption : UIBase
{
    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 버튼 리스너 등록

        // 획득 아이템 아이콘 업데이트
        UpdateOwnedItem();
        // 능력치 1 업데이트
        // 능력치 2 업데이트
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;

        // 버튼 리스너 해제

        // 
    }

    // 보유한 아이템을 업데이트하는 메서드
    private void UpdateOwnedItem()
    {

    }

    //능력치 1에 해당하는 능력치들을 업데이트하는 메서드
    private void UpdateAbilitiesSectorOne()
    {

    }
}
