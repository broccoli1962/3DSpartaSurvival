using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : IWeaponAttack
{
    public void AttackType(WeaponBase weapon)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 attackDir;

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            Vector3 targetPoint = hit.point;
            attackDir = (targetPoint - weapon.transform.position).normalized;
            attackDir.y = 0f;
        }
        else
        {
            attackDir = ray.direction;
            attackDir.y = 0;
        }

        Debug.DrawRay(weapon.transform.position, attackDir * weapon.itemData.AttackRange, Color.red, 1000f);

        //피격 판정
        Vector3 boxCenter = weapon.transform.position + attackDir * (weapon.itemData.AttackRange / 2);
        Vector3 boxRange = new Vector3(1f, 1f, weapon.itemData.AttackRange / 2f);

        Collider[] check = Physics.OverlapBox(boxCenter, boxRange, Quaternion.LookRotation(attackDir), weapon.hitMask);

        foreach (var target in check)
        {
            if (target.TryGetComponent<IDamagable>(out IDamagable enemy))
            {
                enemy.ValueChanged(-weapon.itemData.Power);
            }
            Debug.Log($"{target.name}에 맞음");
        }
    }
}