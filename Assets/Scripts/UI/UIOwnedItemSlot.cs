using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOwnedItemSlot : MonoBehaviour
{
    // �ν����Ϳ��� Icon�� Amount ������Ʈ�� �������ݴϴ�.
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;

    // �θ�(UISelectItem)�� ������ ������ �����ϱ� ���� ȣ���� �޼ҵ�
    public void SetItem(Sprite itemIcon, int itemCount)
    {
        if (icon != null)
        {
            icon.sprite = itemIcon;
        }

        if (amount != null)
        {
            // ������ ������ 1���� Ŭ ���� ������ ǥ���ϰ�, 1���� ���� ����ϴ�.
            if (itemCount > 1)
            {
                amount.gameObject.SetActive(true);
                amount.text = $"x{itemCount}";
            }
            else
            {
                amount.gameObject.SetActive(false);
            }
        }
    }
}
