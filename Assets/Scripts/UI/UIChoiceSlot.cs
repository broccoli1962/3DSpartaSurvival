using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChoiceSlot : MonoBehaviour
{
    private ItemData _itemData;
    private UISelectItem _uiSelectItem;

    // UI ��ҵ� (�ν����Ϳ��� ����)
    public Image iconImage;
    public TextMeshProUGUI statValueText;
    public Button selectButton;

    public void Initialize(ItemData itemData, UISelectItem uiSelectItem)
    {
        _itemData = itemData;
        _uiSelectItem = uiSelectItem;

        // UI ������Ʈ
        iconImage.sprite = _itemData.Icon;
        statValueText.text = _itemData.description;

        // ��ư Ŭ�� �̺�Ʈ ����
        selectButton.onClick.RemoveAllListeners(); // ���� ������ ����
        selectButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        // ���� � ���������� ��Ʈ�ѷ����� �˷���
        _uiSelectItem.OnItemSelected(_itemData);
    }
}
