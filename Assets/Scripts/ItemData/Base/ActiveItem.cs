using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ActiveItem")]
public class ActiveItem : ItemData
{
    [field: SerializeField] public float VariationValue { get; private set; }
    [field: SerializeField] public EWeaponType WeaponType { get; private set; }
    [field: SerializeField, Range(1, 6)] public int Stack { get; private set; }

    public GameObject WeaponPrefab;

    public float CurrentValue { get; private set; }

    public override void AddItem(Player player) //�÷��̾� ���ڷ� �ޱ�
    {
        this.Clone();
    }

    public void AddStack()
    {
        Stack++;
        CalculateStack();
    }
    public void CalculateStack()
    {
        foreach(var values in Ability)
        {
            CurrentValue = values.Value + ((Stack - 1) * VariationValue);
        }
    }
}

//������ ������ 3���߿� �ϳ� ����, �� 6������ ������ ��ø����