using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private LudumDare57 _inputActions;

    public Action<Vector2> MovementEvent;
    public Action StopMovementEvent;

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Awake()
    {
        _inputActions = new();
    }

    private void Start()
    {
        _inputActions.Player.Move.performed += HandldeMovement;
        _inputActions.Player.Move.canceled += HandleStopMovement;
    }

    private void HandldeMovement(InputAction.CallbackContext inputContext)
    {
        MovementEvent?.Invoke(inputContext.ReadValue<Vector2>());
    }

    private void HandleStopMovement(InputAction.CallbackContext inputContext) 
    {
        StopMovementEvent?.Invoke();
    }
}
