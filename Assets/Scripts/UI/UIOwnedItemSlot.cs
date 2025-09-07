using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOwnedItemSlot : MonoBehaviour
{
    // 인스펙터에서 Icon과 Amount 오브젝트를 연결해줍니다.
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;

    // 부모(UISelectItem)가 아이템 정보를 설정하기 위해 호출할 메소드
    public void SetItem(Sprite itemIcon, int itemCount)
    {
        if (icon != null)
        {
            icon.sprite = itemIcon;
        }

        if (amount != null)
        {
            amount.gameObject.SetActive(true);
            amount.text = $"x{itemCount}";
        }
    }
}
