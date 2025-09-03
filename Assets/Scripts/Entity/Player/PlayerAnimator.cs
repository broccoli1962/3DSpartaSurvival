using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

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

        //float velocityX = localMoveDirection.x;
        //float velocityZ = localMoveDirection.z;

        _animator.SetFloat("VelocityX", localMoveDirection.x);
        _animator.SetFloat("VelocityZ", localMoveDirection.z);
    }
    public void PlayDashAnimation()
    {
        _animator.SetTrigger("Dash");
    }
}