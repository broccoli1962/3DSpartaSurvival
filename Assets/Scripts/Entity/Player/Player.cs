using UnityEngine;
public class Player : MonoBehaviour
{
    //Tooltip의 내용 및 수치는 (김민혁 Notion)상세 기획 참고

    #region 기본 능력치
    [Header("기본 능력치")]
    public int maxHealth = 10;
    public float attackPower = 8f;

    [Header("이동 속도")]
    [Tooltip("캐릭터 기준 정면으로 이동할 때의 속도입니다.")]
    public float forwardSpeed = 5f;

    [Tooltip("캐릭터 기준 후진할 때의 속도입니다.")]
    public float backwardSpeed = 3f;

    [Tooltip("캐릭터 기준 좌우(스트레이핑)로 이동할 때의 속도입니다.")]
    public float strafeSpeed = 4f;
    #endregion

    #region 전투 능력치
    [Header("전투 능력치")]
    [Tooltip("모든 공격의 쿨타임을 %로 감소시킵니다. (0.1 = 10%)")]
    public float cooldownReduction = 0f;

    [Tooltip("공격 시 치명타가 발동할 확률. (0.05 = 5%)")]
    public float critChance = 0.05f;

    [Tooltip("치명타 발동 시 기본 피해량에 추가되는 피해량 배율. (0.5 = 50% 추가)")]
    public float critDamage = 0.5f;
    #endregion

    #region 투사체 능력치
    [Header("투사체 능력치 (원거리 공격)")]
    [Tooltip("투사체가 발사되어 나아가는 속도. (1 = 100%)")]
    public float projectileSpeed = 1f;

    [Tooltip("공격할 때마다 발사되는 투사체의 개수.")]
    public int projectileCount = 1;

    [Tooltip("투사체가 필드 위에 존재하는 시간. (1 = 100%)")]
    public float projectileDuration = 1f;
    #endregion

    #region 범위 능력치
    [Header("범위 능력치")]
    [Tooltip("공격의 범위. (1 = 100%)")]
    public float attackRange = 1f;

    [Tooltip("아이템을 획득하는 범위.")]
    public float itemPickupRange = 1.8f;
    #endregion

    #region 성장 능력치
    [Header("성장 능력치")]
    [Tooltip("경험치를 획득하는 %. (1 = 100%)")]
    public float expGain = 1f;
    #endregion
}