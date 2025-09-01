using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityData
{
    [field: SerializeField] public float Value { get; set; }
    [field: SerializeField] public EAbilityType AbilityType { get; set; }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/ConsumableItem")]
public class ConsumableItem : Item
{
    [field:SerializeField] public List<AbilityData> Ability { get; private set; }

    public override void AddItem()
    {
        base.AddItem();
    }
}