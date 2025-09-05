using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        
        float pCount = weapon.itemData.ProjectileCount;
        float startAngle = 0;
        float angle = 0;

        if (pCount > 1)
        {
            angle = weapon.itemData.ProjectileAngle / (pCount - 1);
            startAngle = -weapon.itemData.ProjectileAngle / 2f;
        }

        for (int i = 0; i < pCount; i++) {
            float currentAngle = startAngle + (i * angle);

            Vector3 nextDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * attackDir;
            Quaternion rot = Quaternion.LookRotation(nextDir) * Quaternion.Euler(90f, 0f, 0f);

            GameObject proj = Object.Instantiate(projectilePrefab, weapon.transform.position + Vector3.up * 1f, rot);
            Projectile p = proj.GetComponent<Projectile>();
            
            p.Init(nextDir, weapon.itemData.Power, weapon.itemData.ProjectileSpeed, weapon.hitMask);
        }
    }
}
