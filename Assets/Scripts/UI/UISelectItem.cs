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
        // 이전에 생성했던 선택지 버튼들을 모두 삭제하여 깨끗한 상태로 만든다 (확인용 로그 추가)
        foreach (Transform child in selectButtonParents)
        {
            Debug.Log($"[UISelectItem] 이전 선택지 버튼 삭제: {child.name}");
            Destroy(child.gameObject);
        }

        List<ItemData> candidateItems = new List<ItemData>();
        Debug.Log($"[UISelectItem] DataManager에 등록된 총 아이템 개수: {DataManager.Instance.itemDatas.Count}");


        foreach (var itemData in DataManager.Instance.itemDatas)
        {
            if (itemData == null) // null 참조 방지
            {
                Debug.LogError("[UISelectItem] DataManager.itemDatas에 null ItemData가 있습니다!");
                continue;
            }

            int currentStack = 0;
            bool hasItem = PlayerItemManager.Instance.ownedItems.TryGetValue(itemData, out currentStack);

            Debug.Log($"[UISelectItem] 아이템 '{itemData.name}' (ID: {itemData.Id}, MaxStack: {itemData.Stack}): " +
                      $"현재 보유 개수: {currentStack}, PlayerItemManager에 존재 여부: {hasItem}");


            if (currentStack < itemData.Stack)
            {
                candidateItems.Add(itemData);
                Debug.Log($"[UISelectItem] 아이템 '{itemData.name}'이 후보에 추가됨. (현재 스택: {currentStack} < 최대 스택: {itemData.Stack})");
            }
            else
            {
                Debug.LogWarning($"[UISelectItem] 아이템 '{itemData.name}'은 후보에서 제외됨. " +
                                 $"(현재 스택: {currentStack} >= 최대 스택: {itemData.Stack})");
            }
        }

        Debug.Log($"[UISelectItem] 최종 선택 가능한 후보 아이템 개수: {candidateItems.Count}개");

        // 후보 리스트를 무작위로 섞음 (이 부분은 원래 코드와 동일)
        var random = new System.Random();
        var shuffledCandidates = new List<ItemData>(candidateItems);
        for (int i = 0; i < shuffledCandidates.Count; i++)
        {
            int rand = random.Next(i, shuffledCandidates.Count);
            (shuffledCandidates[i], shuffledCandidates[rand]) = (shuffledCandidates[rand], shuffledCandidates[i]);
        }

        int choiceCount = Mathf.Min(3, shuffledCandidates.Count);
        Debug.Log($"[UISelectItem] 화면에 표시할 최종 선택지 개수: {choiceCount}개");

        for (int i = 0; i < choiceCount; i++)
        {
            ItemData itemToShow = shuffledCandidates[i];
            GameObject buttonGO = Instantiate(levelUpSelectButtonPrefab, selectButtonParents);

            UIChoiceSlot slot = buttonGO.GetComponent<UIChoiceSlot>();
            if (slot != null)
            {
                slot.Initialize(itemToShow, this);
                Debug.Log($"[UISelectItem] '{itemToShow.name}' 아이템 선택 슬롯 생성 및 초기화 완료.");
            }
            else
            {
                Debug.LogError($"[UISelectItem] levelUpSelectButtonPrefab에 UIChoiceSlot 컴포넌트가 없습니다! 프리팹 확인 필요.");
            }
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
