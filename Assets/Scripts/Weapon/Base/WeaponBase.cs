using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDamagable
{
    public float ValueChanged();
}

public class WeaponBase : MonoBehaviour
{
    public ItemData itemData;
    public LayerMask hitMask;
    private float lastAttackTime;

    Coroutine coroutine;

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
        coroutine = StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        if(itemData.AttackCount <= 0)
        {
            coroutine = null;
            yield break;
        }

        for (int i = 0; i < itemData.AttackCount; i++)
        {
            SingleAttack();

            if (i < itemData.AttackCount - 1)
            {
                if (itemData.AttackSpeed > 0)
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
        Vector3 attackDir = Vector3.zero;

        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            Vector3 targetPoint = hit.point;
            attackDir = (targetPoint - transform.position).normalized;
            attackDir.y = 0f;
        }

        Debug.DrawRay(transform.position, attackDir * itemData.AttackRange, Color.red, 1000f);


        Collider[] check = Physics.OverlapBox(transform.position + attackDir * (itemData.AttackRange/2),
            new Vector3(1f, 1f, itemData.AttackRange), Quaternion.LookRotation(attackDir), hitMask);
        
        foreach(var t in check)
        {
            Debug.Log($"{t.name}¿¡ ¸ÂÀ½");
        }
    }
}
