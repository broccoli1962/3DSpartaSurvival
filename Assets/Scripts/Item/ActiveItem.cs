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

//������ ������ 3���߿� �ϳ� ����, �� 6������ ������ ��ø����