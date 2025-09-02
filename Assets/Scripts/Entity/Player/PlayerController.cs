using RPGCharacterAnims.Actions;
using System.Collections; // Coroutine을 사용하기 위해 필요합니다.
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private PlayerInput _playerinputs;
    private PlayerAnimator _playerAnimator;
    private Vector2 moveInput;
    private Rigidbody _rb;
    private Camera _mainCamera;

    [Header("Rotation Settings")]
    public float rotationSpeed = 15f;

    [Header("대쉬 세팅")]
    [Tooltip("대쉬 지속시간 (초 단위)")]
    public float dashDuration = 0.5f;

    private bool isDashing = false; 

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerinputs = new PlayerInput();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _playerinputs.Player.Enable();
        _playerinputs.Player.Dash.performed += OnDash;

        _playerinputs.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        _playerinputs.Player.Disable();
        _playerinputs.Player.Dash.performed -= OnDash;

        _playerinputs.Player.Attack.performed -= OnAttack;
    }

    private void Update()
    {
        moveInput = _playerinputs.Player.Move.ReadValue<Vector2>();
        _playerAnimator.SetMoveAnimation(moveInput);
        RotateTowardsMouse();
    }

    private void FixedUpdate()
    {
        Vector3 worldMoveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 localMoveDirection = transform.InverseTransformDirection(worldMoveDirection);

        float currentSpeed = _player.forwardSpeed;

        if (Mathf.Abs(localMoveDirection.z) > Mathf.Abs(localMoveDirection.x))
        {
            if (localMoveDirection.z > 0)
            {
                currentSpeed = _player.forwardSpeed; // 전진
            }
            else
            {
                currentSpeed = _player.backwardSpeed; // 후진
            }
        }
        else
        {
            currentSpeed = _player.strafeSpeed; // 스트레이핑
        }

        _rb.velocity = worldMoveDirection.normalized * currentSpeed;
    }

    private void RotateTowardsMouse()
    {
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            Vector3 lookDirection = lookPoint - transform.position;
            lookDirection.y = 1f;

            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
    #region Dash 함수
    private void OnDash(InputAction.CallbackContext context)
    {
        if (isDashing)
        {
            return;
        }
        _playerAnimator.PlayDashAnimation();

        StartCoroutine(DashCooldownCoroutine());
    }

    private IEnumerator DashCooldownCoroutine()
    {
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }
    #endregion

    private void OnAttack(InputAction.CallbackContext context)
    {
        _playerAnimator.PerformAttack();
    }
}