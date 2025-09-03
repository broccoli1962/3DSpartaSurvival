using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public Sprite Icon {  get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [TextArea(5, 5)] public string description;
    [field: SerializeField] public EWeaponType weaponType { get; private set; }
    [field: SerializeField] public float Power {  get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttackCount { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float CoolTime { get; private set; }
    [field: SerializeField, Range(1, 3)] public int Stack {  get; private set; }

    public ItemData Clone()
    {
        ItemData newItem = Instantiate(this);
        return newItem;
    }
}