using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectItem : UIBase
{
    [field: SerializeField] public GameObject levelUpSelectButtonPrefab { get; private set; } // ������ ���� ��ư ������
    [field: SerializeField] public Transform selectButtonParents { get; private set; } // ���� ��ư ��ġ

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
