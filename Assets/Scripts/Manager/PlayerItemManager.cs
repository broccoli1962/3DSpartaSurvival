using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : Singleton<PlayerItemManager>
{
    // 플레이어가 가진 아이템, 갯수
    public Dictionary<ItemData, int> ownedItems { get; private set; } = new Dictionary<ItemData, int>();

    public bool AddItem(ItemData itemToAdd)
    {
        if (itemToAdd == null) return false;

        int currentStack = 0;
        ownedItems.TryGetValue(itemToAdd, out currentStack);

        // 설계도(itemToAdd.Stack)에 적힌 최대 스택과 현재 스택(currentStack)을 비교
        if (currentStack >= itemToAdd.Stack)
        {
            Debug.LogWarning($"[PlayerItemManager] {itemToAdd.name}은(는) 이미 최대 스택({itemToAdd.Stack})에 도달하여 추가할 수 없습니다.");
            return false; // 추가 실패
        }

        ownedItems[itemToAdd] = currentStack + 1;

        Debug.Log($"[PlayerItemManager] {itemToAdd.name} 획득. 현재 스택: {ownedItems[itemToAdd]}/{itemToAdd.Stack}");
        return true; // 추가 성공
    }
}
