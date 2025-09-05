using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/PassiveItemSO")]
public class PassiveItemData : ItemData
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MoveSpeed { get; private set; }
}
