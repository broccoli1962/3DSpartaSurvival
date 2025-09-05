using UnityEngine;
using UnityEngine.AI;

public class L_EnemyController : EnemyController
{
    public GameObject _projectilePrefab;
    public Transform _firePoint;         

    private float tooCloseDistance;

    protected override void Start()
    {
        base.Start();
        tooCloseDistance = _monsterData.attackRange / 2f;
    }

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
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceToPlayer < tooCloseDistance)
        {
            // 너무 가까움: 플레이어로부터 멀어지는 방향으로 이동
            Vector3 directionAwayFromPlayer = (transform.position - _playerTarget.position).normalized;
            _agent.SetDestination(transform.position + directionAwayFromPlayer);
        }
        else if (distanceToPlayer > _monsterData.attackRange)
        {
            _agent.SetDestination(_playerTarget.position);
        }
        else
        {
            _agent.SetDestination(transform.position);
        }
    }

    private void HandleAttack()
    {
        transform.LookAt(_playerTarget);

        if (Time.time >= lastAttackTime + _monsterData.attackSpeed)
        {
            lastAttackTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_projectilePrefab != null && _firePoint != null)
        {
            Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        }
    }
}