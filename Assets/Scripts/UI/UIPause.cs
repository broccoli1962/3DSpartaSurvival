using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
    // 능력치 인스펙터 연결
    [field: Header("능력치 표시 관련")]
    [field: SerializeField] public TextMeshProUGUI[] statNames { get; private set; } // 인스펙터에서 StatName 1~6 연결
    [field: SerializeField] public TextMeshProUGUI[] statValues { get; private set; } // 인스펙터에서 StatValue 1~6 연결

    // 버튼 인스펙터 연결
    [field: Header("버튼 연결")]
    [field: SerializeField] public Button btnContinue { get; private set; }
    [field: SerializeField] public Button btnOption { get; private set; }
    [field: SerializeField] public Button btnRestart { get; private set; }
    [field: SerializeField] public Button btnMainMenu { get; private set; }


    // UISelectItem과 동일함
    [field: Header("보유 아이템 관련")]
    [field: SerializeField] public Transform ownedItemIconParent { get; private set; } // 하단 보유 아이템 아이콘 위치
    [field: SerializeField] public GameObject ownedItemIconPrefab { get; private set; } // 하단 아이콘 프리팹

    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 버튼 리스너 등록

        // 획득 아이템 아이콘 업데이트
        UpdateOwnedItem();
        // 능력치 1 업데이트
        UpdateAbilitiesSectorOne();
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;

        // 버튼 리스너 해제

        // 
    }

    // 보유한 아이템을 업데이트하는 메서드
    private void UpdateOwnedItem()
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

    //능력치 1에 해당하는 능력치들을 업데이트하는 메서드
    private void UpdateAbilitiesSectorOne()
    {

    }
}
