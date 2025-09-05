using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossHitBox : MonoBehaviour
{
    [SerializeField] private EnemyBoss owner;

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;

        owner.OnHitBox(other);
    }
}
