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

    // 능력치 1 배열
    private EStatType[] abilitiesSectorOne = new EStatType[]
    {
        EStatType.ProjectileSpeed,
        EStatType.ProjectileCount,
        EStatType.ProjectileAngle, // 투사체 지속 시간으로 변경하거나, Time과 Angle 중 하나 선택
        EStatType.AttackRange,
    };

    // 능력치 2 배열
    private EStatType[] abilitiesSectorTwo = new EStatType[]
    {
        EStatType.Health,
        EStatType.Power,
        EStatType.MoveSpeed,
        EStatType.CoolTime, // 재사용 대기 시간 감소는 보통 CoolTime에 반비례
    };

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



    // ---------------------
    /// <summary>
    /// EStatType에 따라 표시될 한글 이름을 반환합니다.
    /// </summary>
    private string GetStatDisplayName(EStatType statType)
    {
        switch (statType)
        {
            case EStatType.Health: return "최대 체력";
            case EStatType.MoveSpeed: return "이동 속도";
            case EStatType.Power: return "공격력";
            case EStatType.AttackRange: return "공격 범위";
            case EStatType.CoolTime: return "재사용 대기 시간 감소"; // CoolTime 감소로 표시하는 것이 일반적
            case EStatType.AttackSpeed: return "공격 속도";
            case EStatType.AttackCount: return "공격 횟수";
            case EStatType.ProjectileSpeed: return "투사체 속도";
            case EStatType.ProjectileCount: return "투사체 개수";
            case EStatType.ProjectileAngle: return "투사체 각도"; // 또는 지속시간으로 변경
            default: return statType.ToString(); // 정의되지 않은 스탯은 Enum 이름을 그대로 사용
        }
    }

    /// <summary>
    /// EStatType과 값에 따라 적절한 포맷과 단위를 적용하여 문자열을 반환합니다.
    /// </summary>
    private string FormatStatValue(EStatType statType, float value)
    {
        switch (statType)
        {
            case EStatType.Health:
            case EStatType.Power:
            case EStatType.MoveSpeed: return value.ToString("F1"); // 소수점 한 자리
            case EStatType.CoolTime: return $"{value:F1}%"; // 퍼센트 단위
            case EStatType.AttackRange: return $"{value:F1}m"; // 미터 단위
            case EStatType.ProjectileSpeed:
            case EStatType.ProjectileCount:
            case EStatType.ProjectileAngle: return value.ToString("F0"); // 정수
            default: return value.ToString();
        }
    }
}
