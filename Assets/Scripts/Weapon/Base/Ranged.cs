using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : IWeaponAttack
{
    public GameObject projectilePrefab;

    public Ranged(GameObject prefab)
    {
        projectilePrefab = prefab;
    }

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

        Quaternion rot = Quaternion.LookRotation(attackDir) * Quaternion.Euler(90f, 0f, 0f);
        GameObject proj = Object.Instantiate(projectilePrefab, weapon.transform.position + Vector3.up * 1f, rot);
        Projectile p = proj.GetComponent<Projectile>();
        p.Init(attackDir, weapon.itemData.Power, weapon.itemData.AttackSpeed, weapon.hitMask);
    }
}
