using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    [Header("데이터")]
    public MonsterData _monsterData;

    [Header("UI 설정")]
    public GameObject hpBarPrefab;
    public Transform hpBarAnchor;

    [Header("드랍 아이템")]
    public GameObject experienceGemPrefab;

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
        if (hpBarController != null)
        {
            hpBarController.UpdateHP(currentHealth, _monsterData.maxHealth);
        }
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMonsterKilled(this.gameObject);
        }
        if (experienceGemPrefab != null)
        {
            Instantiate(experienceGemPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log(_monsterData.monsterName + " has died.");
        Destroy(gameObject);
    }
}