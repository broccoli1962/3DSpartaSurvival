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
            // ���� ���� �� �÷��̾� ����
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
            // TODO: ���� �÷��̾�� �������� �ִ� ����
            // TODO: ���� �ִϸ��̼� ���

            // ��ȹ�� ����: ���� �� ���� ���ݱ��� �̵� �Ұ�
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
