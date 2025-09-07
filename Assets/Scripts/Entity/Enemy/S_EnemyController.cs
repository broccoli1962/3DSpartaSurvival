using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_EnemyController : EnemyController
{
    private bool canMove = true;

    protected override void Update()
    {
        base.Update();

        if (_playerTarget == null)
        {
            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = true;
            }
            return; 
        }

        if (!canMove) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.transform.position);

        if (distanceToPlayer <= _monsterData.attackRange)
        {
            Attack();
        }
        else
        {

        }
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + _monsterData.attackSpeed)
        {
            lastAttackTime = Time.time;
            Debug.Log(_monsterData.attackPower);

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canMove = false;
        _agent.isStopped = true; // 공격 중에는 확실히 멈추도록 설정
        yield return new WaitForSeconds(_monsterData.attackSpeed);
        canMove = true;
        _agent.isStopped = false; // 다시 움직일 수 있도록 설정
    }
}