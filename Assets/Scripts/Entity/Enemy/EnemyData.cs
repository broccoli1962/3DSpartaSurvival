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
    [Header("기본 정보")]
    public MonsterType monsterType;
    public string monsterName;

    [Header("능력치")]
    public int maxHealth;
    public int attackPower; // 힐러의 경우 힐량으로 사용됩니다.
    public float attackSpeed; // 공격 Delay (초 단위)
    public float attackRange; // 근접/원거리 공격 또는 힐 사거리
    public float moveSpeed;
    public int experienceDrop;

    [Header("UI 설정")]
    public GameObject hpBarPrefab;
    public Transform hpBarAnchor;
    private MonHPBarController monhpBarController;
}