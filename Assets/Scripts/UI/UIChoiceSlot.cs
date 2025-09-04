using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChoiceSlot : MonoBehaviour
{
    private ItemData _itemData;
    private UISelectItem _uiSelectItem;

    // UI 요소들 (인스펙터에서 연결)
    public Image iconImage;
    public TextMeshProUGUI statValueText;
    public Button selectButton;

    public void Initialize(ItemData itemData, UISelectItem uiSelectItem)
    {
        _itemData = itemData;
        _uiSelectItem = uiSelectItem;

        // UI 업데이트
        iconImage.sprite = _itemData.Icon;
        statValueText.text = _itemData.description;

        // 버튼 클릭 이벤트 연결
        selectButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        selectButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        // 내가 어떤 아이템인지 컨트롤러에게 알려줌
        _uiSelectItem.OnItemSelected(_itemData);
    }
}
