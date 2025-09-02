using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private int attackCount = 0;
    private Coroutine resetComboCoroutine;

    [Header("콤보 세팅")]
    [Tooltip("유효 콤보 어택 시간 (초).")]
    public float comboResetTime = 0.8f;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetMoveAnimation(Vector2 moveInput)
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        _animator.SetBool("isMoving", isMoving);

        Vector3 worldMoveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 localMoveDirection = transform.InverseTransformDirection(worldMoveDirection);

        _animator.SetFloat("VelocityX", localMoveDirection.x);
        _animator.SetFloat("VelocityZ", localMoveDirection.z);
    }
    public void PlayDashAnimation()
    {
        _animator.SetTrigger("Dash");
    }

    #region Attack 관련 함수
    public void PerformAttack()
    {
        if (resetComboCoroutine != null)
        {
            StopCoroutine(resetComboCoroutine);
        }

        attackCount++;

        _animator.SetInteger("AttackCount", attackCount);
        _animator.SetBool("Attack", true);

        Invoke("ResetAttackBool", 0.1f);


        resetComboCoroutine = StartCoroutine(ResetCombo());
    }
    private void ResetAttackBool()
    {
        _animator.SetBool("Attack", false);
    }
    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(comboResetTime);
        attackCount = 0;
    }
    #endregion
}