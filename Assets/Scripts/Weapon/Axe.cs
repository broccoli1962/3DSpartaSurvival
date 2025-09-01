using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//test
interface IDamagable
{
    public float ValueChanged(float value);
}

public class Axe : WeaponBase
{
    [SerializeField] private List<Collider> _hittedTarget = new(); 
    public override void Init(ItemData data)
    {
        _itemData = data as ActiveItem;
    }
 
    public override void Attack()
    {
        //마우스 방향으로 공격해야함 이거 아닌거 같다 전부 수정해야할듯
        Collider[] hitTarget = Physics.OverlapBox(_itemData.WeaponPrefab.transform.position, _itemData.WeaponPrefab.transform.localScale, _itemData.WeaponPrefab.transform.rotation, _targetMask);
        if (hitTarget.Length != 0)
        {
            if (!_hittedTarget.Contains(hitTarget[0]))
            {
                if (hitTarget[0].TryGetComponent<IDamagable>(out IDamagable target))
                {
                    target.ValueChanged(-_itemData.CurrentValue);
                }
                Debug.Log(hitTarget[0].name + "데미지 주기"); //데미지 주는 로직 작성
                _hittedTarget.Add(hitTarget[0]);
            }
        }
    }

    IEnumerator AttackSequence()
    {
        return null;
    }
}