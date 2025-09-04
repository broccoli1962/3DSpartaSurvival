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
        //몬스터가 사거리 원 범위 안에 들어와있는가?
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


    //애니메이션이 동작하는 시간을 측정해서
    //상태를 체크할 수 있다.
    //애니메이터 상태에서 코드


    private IEnumerator AttackSequence()
    {
        if (itemData.AttackCount <= 0)
        {
            coroutine = null;
            yield break;
        }

        //애니메이션의 길이 = 다음 공격까지 걸리는 시간
        //애니메이션의 속도를 높이면 길이가 줄어듬
        float animSpeed = animator.GetFloat(AnimParam.AttackSpeedMul);
        float animPlayTime = attackClip.length / animSpeed;

        Debug.Log($"{animPlayTime} 애니메이션 속도에 따른 길이");
        Debug.Log($"{attackClip.length} 애니메이션 클립 길이");

        int temp = 0;
        
        while (temp < itemData.AttackCount)
        {
            attackStretegy.AttackType(this); //전략에 따라서 공격 형태 가짐
            animator.SetTrigger(AnimParam.Attack);

            yield return new WaitForSeconds(animPlayTime); //다음 애니메이션 까지 대기시간
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

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); // 에디터용 마우스 위치 계산
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
            attackDir = transform.forward; // 방향이 없는 경우 기본값으로 앞쪽을 사용
        }

        // 공격 박스 그리기
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + attackDir * (itemData.AttackRange / 2f),
                                                 Quaternion.LookRotation(attackDir),
                                                 Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(2f, 2f, itemData.AttackRange));
    }
#endif
}