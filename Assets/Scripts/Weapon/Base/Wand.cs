using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : IWeaponAttack
{
    public GameObject projectilePrefab;
    public Transform nearbyTransform;
    public Wand(GameObject prefab)
    {
        projectilePrefab = prefab;
    }

    public void AttackType(WeaponBase weapon)
    {
        Collider[] targets = Physics.OverlapSphere(weapon.transform.position, weapon.itemData.AttackRange, weapon.hitMask);

        if(targets.Length > 0)
        {
            nearbyTransform = targets[0].transform;
        }
        
        float pCount = weapon.itemData.ProjectileCount;
        float startAngle = 0;
        float angle = 0;

        if (pCount > 1)
        {
            angle = weapon.itemData.ProjectileAngle / (pCount - 1);
            startAngle = -weapon.itemData.ProjectileAngle / 2f;
        }

        for (int i = 0; i < pCount; i++)
        {
            float currentAngle = startAngle + (i * angle);

            Vector3 nextDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * weapon.transform.forward;
            Quaternion rot = Quaternion.LookRotation(nextDir);

            GameObject proj = Object.Instantiate(projectilePrefab, weapon.transform.position + (Vector3.up * 1f), rot);
            ProjectileChase p = proj.GetComponent<ProjectileChase>();

            p.Init(nearbyTransform, weapon.itemData.Power, weapon.itemData.ProjectileSpeed, weapon.hitMask);
        }
    }
}
