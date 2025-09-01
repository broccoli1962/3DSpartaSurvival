using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private PlayerInput _playerinputs;
    private Vector2 moveInput;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerinputs = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerinputs.Player.Enable();
        _playerinputs.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        _playerinputs.Player.Disable();
        _playerinputs.Player.Dash.performed -= OnDash;
    }

    private void Update()
    {
        moveInput = _playerinputs.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        transform.Translate(movement * _player.moveSpeed * Time.deltaTime);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash!");
    }
}