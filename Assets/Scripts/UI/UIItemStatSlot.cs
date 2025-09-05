using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemStatSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValue;

    // �θ�(UISelectItem)�� ȣ������ �޼ҵ�
    public void SetStat(string statName, float value)
    {
        // ���� ���� ���� �̸� �ڿ� �ݷ�(:)�� �ٿ��ݴϴ�.
        statText.text = statName + ":";

        // �Ҽ��� ù° �ڸ������� ǥ���ϰ�, �׻� + ��ȣ�� �ٿ��ݴϴ�.
        statValue.text = $"+{value:F1}";
    }
}
