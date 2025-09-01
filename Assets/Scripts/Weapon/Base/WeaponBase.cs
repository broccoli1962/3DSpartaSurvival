using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected ActiveItem _itemData;
    protected LayerMask _targetMask;

    public abstract void Init(ItemData data);
    public abstract void Attack();
}
