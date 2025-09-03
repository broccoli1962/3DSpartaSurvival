using UnityEngine;
public enum MonsterType
{
    Short,
    Long,
    Healer,
    Boss
}

[CreateAssetMenu(fileName = "New MonsterData", menuName = "Monster/Create Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("�⺻ ����")]
    public MonsterType monsterType;
    public string monsterName;

    [Header("�ɷ�ġ")]
    public int maxHealth;
    public int attackPower; // ������ ��� �������� ���˴ϴ�.
    public float attackSpeed; // ���� Delay (�� ����)
    public float attackRange; // ����/���Ÿ� ���� �Ǵ� �� ��Ÿ�
    public float moveSpeed;
    public int experienceDrop;

    [Header("UI ����")]
    public GameObject hpBarPrefab;
    public Transform hpBarAnchor;
    private MonHPBarController monhpBarController;
}