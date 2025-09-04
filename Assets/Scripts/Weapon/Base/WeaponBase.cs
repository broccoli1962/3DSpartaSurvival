using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public interface IDamagable
{
    public float ValueChanged(float value);
}

public class WeaponBase : MonoBehaviour
{
    public ItemData itemData;
    public LayerMask hitMask;
    public Animator animator;
    public AnimationClip attackClip;

    private float lastAttackTime;
    private Coroutine coroutine;

    public IWeaponAttack attackStretegy;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        switch (itemData.weaponType)
        {
            case EWeaponType.Melee:
                attackStretegy = new Melee();
                break;
            case EWeaponType.Bow:
                GameObject prj = Resources.Load<GameObject>("Projectile/Arrow");
                Debug.Log(prj.name);
                attackStretegy = new Ranged(prj);
                break;
        }
    }

    private void Update()
    {
        if(coroutine == null && Time.time >= lastAttackTime + itemData.CoolTime)
        {
            if (InRangeEnemy())
            {
                Attack();
            }
        }
    }

    public bool InRangeEnemy()
    {
        //���Ͱ� ��Ÿ� �� ���� �ȿ� �����ִ°�?
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, itemData.AttackRange, hitMask);

        if (hitTargets.Length != 0) {
            return true;
        }
        return false;
    }

    public void Attack()
    {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        if(itemData.AttackSpeed > 0)
        {
            animator.SetFloat(AnimParam.AttackSpeedMul, itemData.AttackSpeed);
        }
        else
        {
            animator.SetFloat(AnimParam.AttackSpeedMul, 1f);
        }

        coroutine = StartCoroutine(AttackSequence());
    }


    //�ִϸ��̼��� �����ϴ� �ð��� �����ؼ�
    //���¸� üũ�� �� �ִ�.
    //�ִϸ����� ���¿��� �ڵ�


    private IEnumerator AttackSequence()
    {
        if (itemData.AttackCount <= 0)
        {
            coroutine = null;
            yield break;
        }

        //�ִϸ��̼��� ���� = ���� ���ݱ��� �ɸ��� �ð�
        //�ִϸ��̼��� �ӵ��� ���̸� ���̰� �پ��
        float animSpeed = animator.GetFloat(AnimParam.AttackSpeedMul);
        float animPlayTime = attackClip.length / animSpeed;

        Debug.Log($"{animPlayTime} �ִϸ��̼� �ӵ��� ���� ����");
        Debug.Log($"{attackClip.length} �ִϸ��̼� Ŭ�� ����");

        int temp = 0;
        
        while (temp < itemData.AttackCount)
        {
            attackStretegy.AttackType(this); //������ ���� ���� ���� ����
            animator.SetTrigger(AnimParam.Attack);

            yield return new WaitForSeconds(animPlayTime); //���� �ִϸ��̼� ���� ���ð�
            temp++;
        }

        lastAttackTime = Time.time;
        coroutine = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (itemData == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, itemData.AttackRange);

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); // �����Ϳ� ���콺 ��ġ ���
        Vector3 attackDir;

        if (new Plane(Vector3.up, transform.position).Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            attackDir = (targetPoint - transform.position).normalized;
            attackDir.y = 0f;
        }
        else
        {
            attackDir = ray.direction;
            attackDir.y = 0;
        }

        if (attackDir.sqrMagnitude < 0.01f)
        {
            attackDir = transform.forward; // ������ ���� ��� �⺻������ ������ ���
        }

        // ���� �ڽ� �׸���
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + attackDir * (itemData.AttackRange / 2f),
                                                 Quaternion.LookRotation(attackDir),
                                                 Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(2f, 2f, itemData.AttackRange));
    }
#endif
}