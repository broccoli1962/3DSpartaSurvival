using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : Singleton<PlayerItemManager>
{
    // 플레이어가 가진 아이템, 갯수
    public Dictionary<ItemData, int> ownedItems { get; private set; } = new Dictionary<ItemData, int>();

    private Dictionary<EStatType, Func<ItemData, float>> statCalculators;

    private Transform playerTransform;

    private void Awake()
    {
        var checkInstance = Instance;

        if (_instance != this)
        {
            Debug.LogWarning($"[PlayerItemManager] 중복된 인스턴스가 생성되어, '{this.gameObject.name}' 오브젝트를 파괴합니다.");
            Destroy(this.gameObject); // 나는 스스로를 파괴한다.
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // 규칙서를 초기화하는 코드
        InitializeCalculators();
    }

    // 계산해야 할 스탯 초기화
    private void InitializeCalculators()
    {
        statCalculators = new Dictionary<EStatType, Func<ItemData, float>>
        {
            { EStatType.Health,       item => (item is PassiveItemData p) ? p.Health : 0f },
            { EStatType.MoveSpeed,    item => (item is PassiveItemData p) ? p.MoveSpeed : 0f },
            { EStatType.Power,        item => item.Power },
            { EStatType.AttackRange,  item => item.AttackRange },
            { EStatType.CoolTime,     item => item.CoolTime },
            { EStatType.AttackSpeed,  item => (item is ActiveItemData a) ? a.AttackSpeed : 0f },
            { EStatType.AttackCount,  item => (item is ActiveItemData a) ? a.AttackCount : 0f },
            { EStatType.ProjectileSpeed,  item => (item is ActiveItemData a) ? a.ProjectileSpeed : 0f },
            { EStatType.ProjectileCount,  item => (item is ActiveItemData a) ? a.ProjectileCount : 0f },
            { EStatType.ProjectileAngle,  item => (item is ActiveItemData a) ? a.ProjectileAngle : 0f },
        };
    }

    // 아이템을 추가하는 메서드 (외부 호출용)
    public bool AddItem(ItemData itemToAdd)
    {
        if (itemToAdd == null) return false;

        int currentStack = 0;
        ownedItems.TryGetValue(itemToAdd, out currentStack);

        // 설계도에 적힌 최대 스택과 현재 스택(currentStack)을 비교
        if (currentStack >= itemToAdd.Stack)
        {
            Debug.LogWarning($"[PlayerItemManager] {itemToAdd.name}은(는) 이미 최대 스택({itemToAdd.Stack})에 도달하여 추가할 수 없습니다.");
            return false; // 추가 실패
        }
        // 최대 스택 미만이라면, 현재 스택을 1 증가시킴
        ownedItems[itemToAdd] = currentStack + 1;

        Debug.Log($"[PlayerItemManager] {itemToAdd.name} 획득. 현재 스택: {ownedItems[itemToAdd]}/{itemToAdd.Stack}");

        if (itemToAdd is ActiveItemData activeItem)
        {
            EquipWeapon(activeItem);
        }

        return true; // 추가 성공
    }

    public void ClearItems()
    {
        ownedItems.Clear();
    }

    // 받은 스탯 변수의 토탈 스탯을 계산하는 메서드 (외부 호출용)
    public float GetTotalStatValue(EStatType statType)
    {
        float total = 0f;
        if (statCalculators.TryGetValue(statType, out var calculator))
        {
            foreach (var itemPair in ownedItems)
            {
                total += calculator(itemPair.Key) * itemPair.Value;
            }
        }
        return total;
    }


    // 무기 아이템이라면 플레이어에게 장착하도록 하는 메서드
    private void EquipWeapon(ActiveItemData weaponData)
    {
        if (playerTransform == null)
        {
            // PlayerManager를 통해 Player를 찾는 것이 FindObjectOfType보다 더 효율적이고 좋습니다.
            if (PlayerManager.Instance != null && PlayerManager.Instance.Player != null)
            {
                playerTransform = PlayerManager.Instance.Player.transform;
            }
            else
            {
                Debug.LogError("[PlayerItemManager] Player를 찾을 수 없어 무기를 생성할 수 없습니다!");
                return;
            }
        }
        if (weaponData.weaponPrefab != null)
        {
            GameObject weaponInstance = Instantiate(weaponData.weaponPrefab, playerTransform);

            WeaponBase weaponBase = weaponInstance.GetComponent<WeaponBase>();

            if (weaponBase != null)
            {
                weaponBase.itemData = weaponData;
                // 스크립트 활성화
                weaponBase.enabled = true;

                Debug.Log($"[PlayerItemManager] {weaponData.ItemName} 무기를 플레이어에게 장착하고 활성화했습니다!");
            }
            else
            {
                Debug.LogError($"[PlayerItemManager] {weaponData.ItemName}의 프리팹에 WeaponBase 스크립트가 없습니다!");
            }
        }
        else
        {
            Debug.LogWarning($"[PlayerItemManager] {weaponData.ItemName}에 weaponPrefab이 연결되어 있지 않습니다!");
        }
    }
}
