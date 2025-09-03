using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDamagable
{
    public float ValueChanged(float value);
}

public class WeaponBase : MonoBehaviour
{
    public ItemData itemData;
    public LayerMask hitMask;
    private float lastAttackTime;
    public Animator animator;

    Coroutine coroutine;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(coroutine == null && Time.time >= lastAttackTime + itemData.CoolTime)
        {
            if (InRangeEnemy())
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    public bool InRangeEnemy()
    {
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

    private IEnumerator AttackSequence()
    {
        if(itemData.AttackCount <= 0)
        {
            coroutine = null;
            yield break;
        }

        for (int i = 0; i < itemData.AttackCount; i++) //���� ����, �ִϸ��̼� ó�� �ʿ�
        {
            SingleAttack();
            animator.SetTrigger(AnimParam.Attack);

            if (i < itemData.AttackCount - 1)
            {
                if (itemData.AttackSpeed >= 0)
                {
                    yield return new WaitForSeconds(1f / itemData.AttackSpeed);
                }
            }
        }
        coroutine = null;
    }
    
    private void SingleAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 attackDir;

        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            Vector3 targetPoint = hit.point;
            attackDir = (targetPoint - transform.position).normalized;
            attackDir.y = 0f;
        }
        else
        {
            attackDir = ray.direction;
            attackDir.y = 0;
        }

        Debug.DrawRay(transform.position, attackDir * itemData.AttackRange, Color.red, 1000f);

        //�ǰ� ����
        Vector3 boxCenter = transform.position + attackDir * (itemData.AttackRange / 2);
        Vector3 boxRange = new Vector3(1f, 1f, itemData.AttackRange / 2f);

        Collider[] check = Physics.OverlapBox(boxCenter, boxRange, Quaternion.LookRotation(attackDir), hitMask);

        foreach (var target in check)
        {
            if (target.TryGetComponent<IDamagable>(out IDamagable enemy))
            {
                enemy.ValueChanged(-itemData.Power);
            }
            Debug.Log($"{target.name}�� ����");
        }
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