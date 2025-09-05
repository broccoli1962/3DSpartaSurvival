using UnityEngine;

public class Heal_EnemyController : EnemyController
{
    private L_EnemyController healTarget;
    private float followDistance = 3f;

    protected override void Update()
    {
        base.Update();
        FindHealTarget();
        FollowAndHeal();
    }

    private void FindHealTarget()
    {
        // TODO: ���� ����ȭ�� ���� �� �κ��� �� ������ �������� �ʴ� ���� �����ϴ�.
        //       (��: 1�ʿ� �� ������ ����)
        L_EnemyController[] allRangedMonsters = FindObjectsOfType<L_EnemyController>();
        L_EnemyController closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (var rangedMonster in allRangedMonsters)
        {
            float distance = Vector3.Distance(transform.position, rangedMonster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = rangedMonster;
            }
        }
        healTarget = closestTarget;
    }

    private void FollowAndHeal()
    {
        if (healTarget == null)
        {
            _agent.isStopped = true;
            return;
        }

        _agent.isStopped = false;
        _agent.SetDestination(healTarget.transform.position);
        _agent.stoppingDistance = followDistance; 


        if (Time.time >= lastAttackTime + _monsterData.attackSpeed)
        {
            lastAttackTime = Time.time;
            healTarget.ReceiveHeal(_monsterData.attackPower);
            Debug.Log(healTarget.name);
        }
    }
}
