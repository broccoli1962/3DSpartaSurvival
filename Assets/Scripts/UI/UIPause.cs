using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPause : UIBase
{
    // 능력치 인스펙터 연결
    [field: Header("능력치 표시 관련")]
    [field: SerializeField] public TextMeshProUGUI[] statNames { get; private set; } // 인스펙터에서 StatName 1~4 연결
    [field: SerializeField] public TextMeshProUGUI[] statValues { get; private set; } // 인스펙터에서 StatValue 1~4 연결

    // 버튼 인스펙터 연결
    [field: Header("버튼 연결")]
    [field: SerializeField] public Button btnContinue { get; private set; }
    [field: SerializeField] public Button btnOption { get; private set; }
    [field: SerializeField] public Button btnRestart { get; private set; }
    [field: SerializeField] public Button btnMainMenu { get; private set; }
    [field: SerializeField] public Button btnAbilityOne { get; private set; }
    [field: SerializeField] public Button btnAbilityTwo { get; private set; }

    // 버튼 상태 스프라이트
    [field: Header("능력치 탭 스프라이트")]
    [field: SerializeField] public Sprite abilityTabSelectedSprite { get; private set; }
    [field: SerializeField] public Sprite abilityTabDeselectedSprite { get; private set; }

    // 버튼의 Image 컴포넌트를 저장할 변수 추가
    private Image _btnAbilityOneImage;
    private Image _btnAbilityTwoImage;

    // UISelectItem과 동일함
    [field: Header("보유 아이템 관련")]
    [field: SerializeField] public Transform ownedItemIconParent { get; private set; } // 하단 보유 아이템 아이콘 위치
    [field: SerializeField] public GameObject ownedItemIconPrefab { get; private set; } // 하단 아이콘 프리팹

    // 능력치 1 배열
    private EStatType[] abilitiesSectorOne = new EStatType[]
    {
        EStatType.Health,        // 최대 체력
        EStatType.MoveSpeed,     // 이동 속도
        EStatType.Power,         // 공격력
        EStatType.AttackSpeed,   // 공격 속도
        EStatType.AttackCount,   // 공격 횟수
    };

    // 능력치 2 배열
    private EStatType[] abilitiesSectorTwo = new EStatType[]
    {
        EStatType.AttackRange,     // 공격 범위
        EStatType.CoolTime,        // 재사용 대기 시간 감소
        EStatType.ProjectileSpeed, // 투사체 속도
        EStatType.ProjectileCount, // 투사체 개수
        EStatType.ProjectileAngle, // 투사체 각도
    };

    private void Awake()
    {
        if (btnAbilityOne != null) _btnAbilityOneImage = btnAbilityOne.GetComponent<Image>();
        if (btnAbilityTwo != null) _btnAbilityTwoImage = btnAbilityTwo.GetComponent<Image>();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 버튼 리스너 등록
        btnContinue?.onClick.AddListener(OnClickContinue);
        btnRestart?.onClick.AddListener(OnClickRestart);
        btnMainMenu?.onClick.AddListener(OnClickMainMenu);
        btnAbilityOne?.onClick.AddListener(UpdateAbilitiesSectorOne);
        btnAbilityTwo?.onClick.AddListener(UpdateAbilitiesSectorTwo);
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
        btnContinue?.onClick.RemoveListener(OnClickContinue);
        btnRestart?.onClick.RemoveListener(OnClickRestart);
        btnMainMenu?.onClick.RemoveListener(OnClickMainMenu);
        btnAbilityOne?.onClick.RemoveListener(UpdateAbilitiesSectorOne);
        btnAbilityTwo?.onClick.RemoveListener(UpdateAbilitiesSectorTwo);
        // 
    }

    // ========================================================
    // 버튼 기능 구현

    private void OnClickContinue()
    {
        // 현재 UI를 닫기만 하면 OnClose()에서 Time.timeScale이 1f로 돌아감
        UIManager.Instance.CloseUI<UIPause>();
    }

    private void OnClickRestart()
    {
        // 현재 씬을 다시 로드하여 게임을 재시작
        GameManager.Instance.RestartGame();
    }

    private void OnClickMainMenu()
    {
        // 타이틀 씬으로 돌아감
        GameManager.Instance.MainMenu();
    }


    // =======================================================
    // 정보 표시 기능
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
        UpdateAbilitiesDisplay(abilitiesSectorOne);
        if (_btnAbilityOneImage != null) _btnAbilityOneImage.sprite = abilityTabSelectedSprite;
        if (_btnAbilityTwoImage != null) _btnAbilityTwoImage.sprite = abilityTabDeselectedSprite;
    }

    private void UpdateAbilitiesSectorTwo()
    {
        UpdateAbilitiesDisplay(abilitiesSectorTwo);
        if (_btnAbilityOneImage != null) _btnAbilityOneImage.sprite = abilityTabDeselectedSprite;
        if (_btnAbilityTwoImage != null) _btnAbilityTwoImage.sprite = abilityTabSelectedSprite;
    }

    /// <summary>
    /// 주어진 EStatType 배열에 따라 능력치 표시 UI를 갱신합니다.
    /// </summary>
    private void UpdateAbilitiesDisplay(EStatType[] statTypesToDisplay)
    {
        // 현재 statNames와 statValues 배열이 인스펙터에 잘 연결되어 있는지 확인합니다.

        // 반복문을 사용하여 statTypesToDisplay 배열의 각 EStatType에 접근합니다.

        for (int i = 0; i < statTypesToDisplay.Length; i++)
        {
            if (i >= statNames.Length || i >= statValues.Length)
            {
                Debug.LogWarning("[UIOption] 표시할 능력치 개수가 UI 슬롯 개수보다 많습니다! UI 슬롯을 더 추가해주세요.");
                break; // 더 이상 표시할 UI 슬롯이 없으므로 중단
            }

            EStatType statType = statTypesToDisplay[i];

            // PlayerItemManager.Instance.GetTotalStatValue(statType)을 호출하여 해당 EStatType의 현재 총합 값을 가져옵니다.
            float totalValue = PlayerItemManager.Instance.GetTotalStatValue(statType);

            // statNames[i].text 에 능력치 이름 문자열을 할당합니다.
            statNames[i].text = GetStatDisplayName(statType);

            // statValues[i].text 에 totalValue 값을 문자열로 할당합니다.
            statValues[i].text = FormatStatValue(statType, totalValue);
        }

        // 만약 표시해야 할 능력치 개수(statTypesToDisplay.Length)가 6개 미만이라면,
        for (int i = statTypesToDisplay.Length; i < statNames.Length; i++)
        {
            statNames[i].text = "";
            statValues[i].text = "";
        }
    }

    // ====================================================
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
    /// EStatType과 값에 따라 적절한 포맷과 단위를 적용하여 문자열을 반환
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
