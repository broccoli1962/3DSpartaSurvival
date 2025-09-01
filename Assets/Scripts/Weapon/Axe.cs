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
        //���콺 �������� �����ؾ��� �̰� �ƴѰ� ���� ���� �����ؾ��ҵ�
        Collider[] hitTarget = Physics.OverlapBox(_itemData.WeaponPrefab.transform.position, _itemData.WeaponPrefab.transform.localScale, _itemData.WeaponPrefab.transform.rotation, _targetMask);
        if (hitTarget.Length != 0)
        {
            if (!_hittedTarget.Contains(hitTarget[0]))
            {
                if (hitTarget[0].TryGetComponent<IDamagable>(out IDamagable target))
                {
                    target.ValueChanged(-_itemData.CurrentValue);
                }
                Debug.Log(hitTarget[0].name + "������ �ֱ�"); //������ �ִ� ���� �ۼ�
                _hittedTarget.Add(hitTarget[0]);
            }
        }
    }

    IEnumerator AttackSequence()
    {
        return null;
    }
}