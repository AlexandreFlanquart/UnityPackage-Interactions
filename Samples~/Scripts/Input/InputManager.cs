using System;
using UnityEngine;
using UnityEngine.InputSystem;


public enum ActionMap
{
    PLAYER,
    UI
}

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnPressDirection;
    public event Action<Vector2> OnLookDirection;
    public event Action OnPressInteract;

    private PlayerControls playerControls;
    private PlayerInput playerInput;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    static InputManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Debug.Log("InputManager Start");
        playerInput = GetComponent<PlayerInput>();
        playerControls = new PlayerControls();

        playerControls.Player.Enable();
        playerControls.Player.Move.performed += MovementPerformed;
        playerControls.Player.Move.canceled += MovementCanceled;
        playerControls.Player.Look.performed += LookPerformed;
        playerControls.Player.Look.canceled += LookCanceled;
        playerControls.Player.Interact.performed += InteractPerformed;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }
    private void MovementPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log("Movement Performed : " + context.ReadValue<Vector2>());
        movementInput = context.ReadValue<Vector2>();
    }

    private void MovementCanceled(InputAction.CallbackContext context)
    {
        //Debug.Log("Movement Canceled");
        movementInput = Vector2.zero;
    }

    private void LookPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log("look Performed : " + context.ReadValue<Vector2>());
        lookInput = context.ReadValue<Vector2>();
    }

    private void LookCanceled(InputAction.CallbackContext context)
    {
        //Debug.Log("look Canceled");
        lookInput = Vector2.zero;
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Interact Performed");
        OnPressInteract?.Invoke();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            OnPressDirection?.Invoke(movementInput);
        }
        if (lookInput != Vector2.zero)
        {
            OnLookDirection?.Invoke(lookInput);
        }

    }
}
