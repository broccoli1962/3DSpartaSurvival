using UnityEngine;
public class Player : MonoBehaviour
{
    //Tooltip�� ���� �� ��ġ�� (����� Notion)�� ��ȹ ����

    #region �⺻ �ɷ�ġ
    [Header("�⺻ �ɷ�ġ")]
    public int maxHealth = 10;
    public float attackPower = 8f;

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

    #region ���� �ɷ�ġ
    [Header("���� �ɷ�ġ")]
    [Tooltip("����ġ�� ȹ���ϴ� %. (1 = 100%)")]
    public float expGain = 1f;
    #endregion
}