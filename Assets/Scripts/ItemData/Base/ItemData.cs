using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [field: Header("������ ����")]
    [field: SerializeField] public Sprite Icon {  get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [TextArea(5, 5)] public string description;

    [field: Header("������ �������ͽ�")]
    [field: SerializeField] public float Power {  get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float CoolTime { get; private set; }
    [field: SerializeField, Range(1, 3)] public int Stack {  get; private set; }

    public ItemData Clone()
    {
        ItemData newItem = Instantiate(this);
        return newItem;
    }
}