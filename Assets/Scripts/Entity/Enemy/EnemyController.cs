using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyController : MonoBehaviour
{
    public MonsterData _monsterData;

    protected int currentHealth;
    protected NavMeshAgent _agent;
    protected Transform _playerTarget;
    protected Animator _animator;
    protected float lastAttackTime;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        if (_monsterData != null)
        {
            currentHealth = _monsterData.maxHealth;
            _agent.speed = _monsterData.moveSpeed;
        }
    }

    protected virtual void Update()
    {
        if (_playerTarget == null) return;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(_monsterData.monsterName + " has died.");
        // �״� �ִϸ��̼� ���
        // ���� ����ġ �� ������ ��� �߰� ���� ����
        Destroy(gameObject);
    }

    // ������ ���� ���ֱ� ���� �޼���
    public void ReceiveHeal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > _monsterData.maxHealth)
        {
            currentHealth = _monsterData.maxHealth;
        }
    }
}
