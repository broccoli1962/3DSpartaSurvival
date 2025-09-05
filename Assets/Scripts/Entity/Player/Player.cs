using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    //Tooltip�� ���� �� ��ġ�� (����� Notion)�� ��ȹ ����

    #region �⺻ �ɷ�ġ
    [Header("�⺻ �ɷ�ġ")]
    public int maxHealth = 10;
    private int currentHealth;
    public int attackPower = 8;

    [Header("UI ����")]
    public GameObject hpBarPrefab;
    public Transform hpBarAnchor;
    private PlayerHPBarController playerhpBarController;

    [Header("�̵� �ӵ�")]
    [Tooltip("ĳ���� ���� �������� �̵��� ���� �ӵ��Դϴ�.")]
    public float forwardSpeed = 5f;

    [Tooltip("ĳ���� ���� ������ ���� �ӵ��Դϴ�.")]
    public float backwardSpeed = 3f;

    [Tooltip("ĳ���� ���� �¿�(��Ʈ������)�� �̵��� ���� �ӵ��Դϴ�.")]
    public float strafeSpeed = 4f;
    #endregion

    #region ���� �ɷ�ġ
    [Header("���� �ɷ�ġ")]
    [Tooltip("��� ������ ��Ÿ���� %�� ���ҽ�ŵ�ϴ�. (0.1 = 10%)")]
    public float cooldownReduction = 0f;

    [Tooltip("���� �� ġ��Ÿ�� �ߵ��� Ȯ��. (0.05 = 5%)")]
    public float critChance = 0.05f;

    [Tooltip("ġ��Ÿ �ߵ� �� �⺻ ���ط��� �߰��Ǵ� ���ط� ����. (0.5 = 50% �߰�)")]
    public float critDamage = 0.5f;
    #endregion

    #region ����ü �ɷ�ġ
    [Header("����ü �ɷ�ġ (���Ÿ� ����)")]
    [Tooltip("����ü�� �߻�Ǿ� ���ư��� �ӵ�. (1 = 100%)")]
    public float projectileSpeed = 1f;

    [Tooltip("������ ������ �߻�Ǵ� ����ü�� ����.")]
    public int projectileCount = 1;

    [Tooltip("����ü�� �ʵ� ���� �����ϴ� �ð�. (1 = 100%)")]
    public float projectileDuration = 1f;
    #endregion

    #region ���� �ɷ�ġ
    [Header("���� �ɷ�ġ")]
    [Tooltip("������ ����. (1 = 100%)")]
    public float attackRange = 1f;

    [Tooltip("�������� ȹ���ϴ� ����.")]
    public float itemPickupRange = 1.8f;
    #endregion

    #region ���� �ɷ�ġ(����ġ ����)
    [Header("���� �ɷ�ġ")]
    public float expGain = 1f;

    private int currentLevel = 1;
    private int currentExperience = 0;
    private int requiredExpForNextLevel = 100; 

    [Header("����ġ UI")]
    public Slider expSlider;
    public TextMeshProUGUI levelText;
    #endregion

    public event Action OnLevelChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        if (hpBarPrefab != null && hpBarAnchor != null)
        {
            GameObject hpBarInstance = Instantiate(hpBarPrefab, hpBarAnchor);

            playerhpBarController = hpBarInstance.GetComponentInChildren<PlayerHPBarController>();

            if (playerhpBarController != null)
            {
                playerhpBarController.UpdateHP(currentHealth, maxHealth);
            }
        }
        UpdateExpUI();
    }
    public void TakeDamage(int damage)
    {
        if (playerhpBarController == null)
        {
            return;
        }

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        playerhpBarController.UpdateHP(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void AddExperience(int baseAmount)
    {
        int finalAmount = (int)(baseAmount * expGain);
        currentExperience += finalAmount;
        Debug.Log($"����ġ {finalAmount} ȹ��! | �� ����ġ: {currentExperience}");

        UpdateExpUI();

        while (currentExperience >= requiredExpForNextLevel)
        {
            LevelUp();
        }
    }
    private void Die()
    {
        Debug.Log("Die");

        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.ShowGameOverScreen();
        }

        gameObject.SetActive(false);
    }
    private void LevelUp()
    {
        currentExperience -= requiredExpForNextLevel;

        currentLevel++;

        requiredExpForNextLevel = (int)(requiredExpForNextLevel * 1.2f);

        Debug.Log($"���� ��! ���� ����: {currentLevel}");

        UpdateExpUI();

        // ���� ��ųâ ������Ʈ �ʿ�(���Դ�)
        OnLevelChanged?.Invoke();
    }

    private void UpdateExpUI()
    {
        if (expSlider != null)
        {
            // ����ġ ��(�����̴�)�� ���� ������Ʈ (�������ġ / �ʿ����ġ)
            expSlider.value = (float)currentExperience / requiredExpForNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = "Lv. " + currentLevel;
        }
    }
}