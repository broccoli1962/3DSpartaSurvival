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
        // 죽는 애니메이션 재생
        // 이후 경험치 및 아이템 드랍 추가 개발 예정
        Destroy(gameObject);
    }

    // 힐러가 힐을 해주기 위한 메서드
    public void ReceiveHeal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > _monsterData.maxHealth)
        {
            currentHealth = _monsterData.maxHealth;
        }
    }
}
