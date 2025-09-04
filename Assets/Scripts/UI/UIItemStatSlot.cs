using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemStatSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValue;

    // 부모(UISelectItem)가 호출해줄 메소드
    public void SetStat(string statName, float value)
    {
        // 보기 좋게 스탯 이름 뒤에 콜론(:)을 붙여줍니다.
        statText.text = statName + ":";

        // 소수점 첫째 자리까지만 표시하고, 항상 + 기호를 붙여줍니다.
        statValue.text = $"+{value:F1}";
    }
}
