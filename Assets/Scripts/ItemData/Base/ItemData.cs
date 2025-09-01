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

public class ItemData : ScriptableObject
{
    [field: SerializeField] public Sprite Icon {  get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public List<AbilityData> Ability { get; private set; }

    public ItemData Clone()
    {
        ItemData newItem = Instantiate(this);
        return newItem;
    }

    public virtual void AddItem(Player player) {}
}