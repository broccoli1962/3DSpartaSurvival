using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/ActiveItemSO")]
public class ActiveItemData : ItemData
{
    [field: SerializeField] public EWeaponType weaponType { get; private set; }
    [field: SerializeField, Range(0.1f, 2.5f)] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttackCount { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public float ProjectileCount { get; private set; }
    [field: SerializeField, Range(0f, 360f)] public float ProjectileAngle { get; private set; }
}
