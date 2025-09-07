using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChoiceSlot : MonoBehaviour
{
    private ItemData itemData;
    private UISelectItem uiSelectItem;

    // UI 요소들 (인스펙터에서 연결)
    [Header("UI 요소 연결")]
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI statValueText;
    public Button selectButton;

    public void Initialize(ItemData itemData, UISelectItem uiSelectItem)
    {
        this.itemData = itemData;
        this.uiSelectItem = uiSelectItem;

        // UI 업데이트
        iconImage.sprite = itemData.Icon;
        itemNameText.text = $"[{itemData.ItemName}]";

        statValueText.text = FormatItemStats(itemData);

        // 버튼 클릭 이벤트 연결
        selectButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        selectButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        // 내가 어떤 아이템인지 컨트롤러에게 알려줌
        uiSelectItem.OnItemSelected(itemData);
    }

    // 해당 아이템이 가진 스탯을 출력하는 메서드
    // 개선 사항
    private string FormatItemStats(ItemData item)
    {
        StringBuilder statsBuilder = new StringBuilder();

        // 공용 스탯 확인
        if (item.Power > 0) statsBuilder.AppendLine($"공격력 +{item.Power}");
        if (item.AttackRange > 0) statsBuilder.AppendLine($"공격 범위 +{item.AttackRange:F1}m");
        if (item.CoolTime > 0) statsBuilder.AppendLine($"재사용 대기 시간 -{item.CoolTime:F1}%");

        // 액티브 아이템 스탯 확인
        if (item is ActiveItemData activeItem)
        {
            if (activeItem.AttackSpeed > 0) statsBuilder.AppendLine($"공격 속도 +{activeItem.AttackSpeed:F1}%");
            if (activeItem.AttackCount > 0) statsBuilder.AppendLine($"공격 횟수 +{activeItem.AttackCount}");
            if (activeItem.ProjectileSpeed > 0) statsBuilder.AppendLine($"투사체 속도 +{activeItem.ProjectileSpeed:F0}");
            if (activeItem.ProjectileCount > 0) statsBuilder.AppendLine($"투사체 개수 +{activeItem.ProjectileCount}");
            if (activeItem.ProjectileAngle > 0) statsBuilder.AppendLine($"투사체 각도 +{activeItem.ProjectileAngle:F0}°");
        }

        // 패시브 아이템 스탯 확인
        if (item is PassiveItemData passiveItem)
        {
            if (passiveItem.Health > 0) statsBuilder.AppendLine($"최대 체력 +{passiveItem.Health}");
            if (passiveItem.MoveSpeed > 0) statsBuilder.AppendLine($"이동 속도 +{passiveItem.MoveSpeed:F1}");
        }

        return statsBuilder.ToString();
    }
}
