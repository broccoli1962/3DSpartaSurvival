using System.Collections;
using UnityEngine;

public class S_EnemyController : EnemyController
{
    private bool canMove = true;

    protected override void Update()
    {
        base.Update();

        if (!canMove) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceToPlayer <= _monsterData.attackRange)
        {
            _agent.isStopped = true;
            Attack();
        }
        else
        {
            // 공격 범위 밖 플레이어 추적
            //_agent.isStopped = false;
            //_agent.SetDestination(_playerTarget.position);
        }
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + _monsterData.attackSpeed)
        {
            lastAttackTime = Time.time;
            Debug.Log(_monsterData.attackPower);
            // TODO: 실제 플레이어에게 데미지를 주는 로직
            // TODO: 공격 애니메이션 재생

            // 기획서 내용: 공격 후 다음 공격까지 이동 불가
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(_monsterData.attackSpeed);
        canMove = true;
    }
}
