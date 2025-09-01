using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ActiveItem")]
public class ActiveItem : Item
{
    [field: SerializeField] public float BaseValue {  get; private set; }
    [field: SerializeField] public float VariationValue { get; private set; }
    [field: SerializeField] public EWeaponType WeaponType { get; private set; }

    [field: SerializeField, Range(1, 6)] public int Stack { get; private set; }
    [field: SerializeField] public float CoolTime { get; private set; }

    public float CurrentValue { get; private set; }

    public override void AddItem()
    {
        base.AddItem();
    }

    public void AddStack()
    {
        Stack++;
        CalculateStack();
    }
    public void CalculateStack()
    {
        CurrentValue = BaseValue + ((Stack - 1) * VariationValue);
    }
}

//레벨업 했을때 3개중에 하나 선택, 총 6개까지 아이템 중첩가능