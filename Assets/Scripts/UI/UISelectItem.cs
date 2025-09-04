using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectItem : UIBase
{
    [field: SerializeField] public GameObject levelUpSelectButtonPrefab { get; private set; } // 레벨업 선택 버튼 프리팹
    [field: SerializeField] public Transform selectButtonParents { get; private set; } // 선택 버튼 위치

    protected override void OnOpen()
    {
        base.OnOpen();

    }

    protected override void OnClose()
    {
        base.OnClose();

    }

    public void ChoiceItemButton()
    {

    }
}
