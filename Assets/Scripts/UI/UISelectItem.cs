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
    [field: SerializeField] public Transform statSlotParent { get; private set; }   // 스탯 슬롯 위치
    [field: SerializeField] public GameObject statSlotPrefab { get; private set; }  // 스탯 프리팹
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
        // 후보 아이템 리스트 생성
        List<ItemData> candidateItems = new List<ItemData>();

        // 모든 아이템을 대상으로 필터링
        foreach (var itemData in DataManager.Instance.itemDatas)
        {
            int currentStack = 0;
            PlayerItemManager.Instance.ownedItems.TryGetValue(itemData, out currentStack);
            if (currentStack < itemData.Stack)
            {
                candidateItems.Add(itemData);
            }
        }

        // 후보 리스트를 무작위로 섞음
        var random = new System.Random();
        var shuffledCandidates = new List<ItemData>(candidateItems);
        for (int i = 0; i < shuffledCandidates.Count; i++)
        {
            int rand = random.Next(i, shuffledCandidates.Count);
            (shuffledCandidates[i], shuffledCandidates[rand]) = (shuffledCandidates[rand], shuffledCandidates[i]);
        }

        // 최대 3개의 선택지를 화면에 표시
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
        // 이전에 생성했던 스탯 슬롯들을 모두 삭제
        foreach (Transform child in statSlotParent)
        {
            Destroy(child.gameObject);
        }
        // EStatType Enum의 모든 값들을 순회하면서 스탯을 하나씩 표시
        foreach (EStatType statType in Enum.GetValues(typeof(EStatType)))
        {
            // PlayerItemManager에게 해당 스탯의 총합을 물어봄
            float value = PlayerItemManager.Instance.GetTotalStatValue(statType);

            // 스탯 보너스가 0보다 클 때만 UI에 표시 (0인 스탯은 굳이 보여주지 않음)
            if (value > 0)
            {
                // 스탯 슬롯 프리팹(ItemStatSlot.prefab)을 생성
                GameObject slotGO = Instantiate(statSlotPrefab, statSlotParent);

                // 생성된 슬롯에서 UIItemStatSlot 스크립트를 가져옵니다.
                UIItemStatSlot slotScript = slotGO.GetComponent<UIItemStatSlot>();

                // 스크립트의 SetStat 메소드를 호출하여 정보를 설정합니다.
                slotScript.SetStat(statType.ToString(), value);
            }
        }
    }


    // 개선 가능한 사항
    // 어차피 한 게임에서 가진 아이템이 사라지지 않기 때문에 매번 레벨업 때마다 아이콘 슬롯을 지우지 않고 추가하는 방식으로 개선할 수 있을 것 같다.
    private void UpdateOwnedItemsDisplay()
    {
        // 이전에 생성했던 아이콘 슬롯들을 모두 삭제
        foreach (Transform child in ownedItemIconParent)
        {
            Destroy(child.gameObject);
        }

        // PlayerItemManager로부터 현재 보유한 아이템 목록을 가져옴
        var ownedItems = PlayerItemManager.Instance.ownedItems;

        // 보유한 아이템 각각에 대해 아이콘 슬롯을 생성
        foreach (var itemPair in ownedItems)
        {
            ItemData itemData = itemPair.Key;
            int itemCount = itemPair.Value;

            // SelectItemSlot 프리팹을 생성
            GameObject slotGO = Instantiate(ownedItemIconPrefab, ownedItemIconParent);

            // 생성된 슬롯의 스크립트를 가져와서 아이템 정보 설정
            UIOwnedItemSlot slotScript = slotGO.GetComponent<UIOwnedItemSlot>();
            if (slotScript != null)
            {
                slotScript.SetItem(itemData.Icon, itemCount);
            }
        }
    }

    public void OnItemSelected(ItemData selectedItem)
    {
        // 선택된 아이템을 인벤토리에 추가
        PlayerItemManager.Instance.AddItem(selectedItem);

        // 아이템 선택이 끝났으므로 UI를 닫음
        CloseUI();
    }
}
