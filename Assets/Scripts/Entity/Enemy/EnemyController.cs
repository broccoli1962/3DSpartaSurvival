using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    [Header("데이터")]
    public MonsterData _monsterData;

    [Header("UI 설정")]
    public GameObject hpBarPrefab;
    public Transform hpBarAnchor;

    public int currentHealth { get; private set; }
    protected MonHPBarController hpBarController;
    protected NavMeshAgent _agent;
    protected Transform _playerTarget;
    protected Animator _animator;
    protected float lastAttackTime;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerTarget = playerObject.transform;
        }
    }

    protected virtual void Start()
    {
        if (_monsterData != null)
        {
            // MonsterData로부터 능력치 받음.
            currentHealth = _monsterData.maxHealth;
            _agent.speed = _monsterData.moveSpeed;

            if (hpBarPrefab != null && hpBarAnchor != null)
            {
                GameObject hpBarInstance = Instantiate(hpBarPrefab, hpBarAnchor);
                hpBarController = hpBarInstance.GetComponent<MonHPBarController>();
                if (hpBarController != null)
                {
                    hpBarController.UpdateHP(currentHealth, _monsterData.maxHealth);
                    hpBarController.SetName(_monsterData.monsterName);
                }
            }
        }
    }

    protected virtual void Update()
    {
        if (_playerTarget == null) return;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (hpBarController != null)
        {
            hpBarController.UpdateHP(currentHealth, _monsterData.maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ReceiveHeal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > _monsterData.maxHealth)
        {
            currentHealth = _monsterData.maxHealth;
        }

        // HP 바 UI 업데이트
        if (hpBarController != null)
        {
            hpBarController.UpdateHP(currentHealth, _monsterData.maxHealth);
        }
    }

    // 죽었을 때 처리
    protected virtual void Die()
    {
        Debug.Log(_monsterData.monsterName + " has died.");
        // TODO: 죽는 애니메이션 재생, 아이템 드랍 등
        Destroy(gameObject);
    }
}