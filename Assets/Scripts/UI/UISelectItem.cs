using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISelectItem : UIBase
{
    [field: Header("선택지 관련")]
    [field: SerializeField] public GameObject levelUpSelectButtonPrefab { get; private set; } // 레벨업 선택 버튼 프리팹
    [field: SerializeField] public Transform selectButtonParents { get; private set; } // 선택 버튼 위치

    [field: Header("플레이어 정보 표시 관련")]
    [field: SerializeField] public TextMeshProUGUI totalStatsText { get; private set; } // 좌측 스탯 총합 텍스트
    [field: SerializeField] public Transform ownedItemIconParent { get; private set; } // 하단 보유 아이템 아이콘 위치
    [field: SerializeField] public GameObject ownedItemIconPrefab { get; private set; } // 하단 아이콘 프리팹

    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 선택지 생성 및 표시
        ShowChoices();
        // 좌측 스탯 정보 업데이트
        UpdateTotalStatsDisplay();
        // 하단 보유 아이템 목록 업데이트
        UpdateOwnedItemsDisplay();
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;

        // 이전에 생성했던 선택지 버튼들을 모두 삭제하여 깨끗한 상태로 만든다
        foreach (Transform child in selectButtonParents)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowChoices()
    {
        // 1. 후보 아이템 리스트 생성
        List<ItemData> candidateItems = new List<ItemData>();

        // 2. 모든 아이템을 대상으로 필터링
        foreach (var itemData in DataManager.Instance.itemDatas)
        {
            int currentStack = 0;
            PlayerItemManager.Instance.ownedItems.TryGetValue(itemData, out currentStack);
            if (currentStack < itemData.Stack)
            {
                candidateItems.Add(itemData);
            }
        }

        // 3. 후보 리스트를 무작위로 섞음
        var random = new System.Random();
        var shuffledCandidates = new List<ItemData>(candidateItems);
        for (int i = 0; i < shuffledCandidates.Count; i++)
        {
            int rand = random.Next(i, shuffledCandidates.Count);
            (shuffledCandidates[i], shuffledCandidates[rand]) = (shuffledCandidates[rand], shuffledCandidates[i]);
        }

        // 4. 최대 3개의 선택지를 화면에 표시
        int choiceCount = Mathf.Min(3, shuffledCandidates.Count);
        for (int i = 0; i < choiceCount; i++)
        {
            ItemData itemToShow = shuffledCandidates[i];
            GameObject buttonGO = Instantiate(levelUpSelectButtonPrefab, selectButtonParents);

            // ★ 중요: 생성된 버튼(슬롯)에 아이템 정보를 넘겨주고, 클릭했을 때 어떤 함수를 호출할지 알려줘야 합니다.
            // 이 기능을 위해 버튼 프리팹에 UI_ChoiceSlot 같은 별도의 스크립트가 필요합니다.
            UIChoiceSlot slot = buttonGO.GetComponent<UIChoiceSlot>();
            slot.Initialize(itemToShow, this);
        }
    }

    private void UpdateTotalStatsDisplay()
    {
        // PlayerInventory의 OwnedItems를 기반으로 스탯 총합을 계산하고
        // totalStatsText.text에 반영하는 로직
        float totalPower = 0f;
        float totalAttackRange = 0f;

        foreach (var itemPair in PlayerItemManager.Instance.ownedItems)
        {
            ItemData item = itemPair.Key;
            int count = itemPair.Value;
            totalPower += item.Power * count;
            totalAttackRange += item.AttackRange * count;
        }

        totalStatsText.text = $"";
    }

    private void UpdateOwnedItemsDisplay()
    {
        // ownedItemIconParent 아래의 기존 아이콘들을 모두 삭제
        foreach (Transform child in ownedItemIconParent) Destroy(child.gameObject);

        // PlayerInventory의 OwnedItems를 기반으로 아이콘과 개수를 표시
        foreach (var itemPair in PlayerItemManager.Instance.ownedItems)
        {
            GameObject iconGO = Instantiate(ownedItemIconPrefab, ownedItemIconParent);
            // ... iconGO의 이미지와 텍스트를 itemPair.Key(ItemData), itemPair.Value(개수)로 설정 ...
        }
    }

    // 이전에 ChoiceItemButton()으로 비워두셨던 부분입니다.
    // 이제 각 선택지 슬롯이 직접 이 함수를 호출하게 만들어, 어떤 아이템이 선택되었는지 명확히 알 수 있습니다.
    public void OnItemSelected(ItemData selectedItem)
    {
        // 1. 선택된 아이템을 인벤토리에 추가
        PlayerItemManager.Instance.AddItem(selectedItem);

        // 2. 아이템 선택이 끝났으므로 UI를 닫음
        CloseUI();
    }
}
