using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyUnityPackage.Toolkit
{
    public enum ActionMap
    {
        PLAYER,
        UI
    }

    public class InputManager : MonoBehaviour
    {
        public event Action<Vector2> OnPressDirection;
        public event Action OnPressInteract;
        public event Action OnSubmit;
        public event Action<bool> OnSprintPressed;

        private PlayerControls playerControls;
        private PlayerInput playerInput;
        private Vector2 movementInput = Vector2.zero;

        private void Awake()
        {
            ServiceLocator.AddService<InputManager>(gameObject);
        }

        private void Start()
        {
            Debug.Log("InputManager Start");
            playerInput = GetComponent<PlayerInput>();
            playerControls = new PlayerControls();

            playerControls.Player.Enable();
            playerControls.Player.Move.performed += MovementPerformed;
            playerControls.Player.Move.canceled += MovementCanceled;
            playerControls.Player.Interact.performed += InteractPerformed;
            playerControls.Player.Sprint.performed += SprintPerformed;
            playerControls.Player.Sprint.canceled += SprintCanceled;

            //playerControls.UI.Enable();
            playerControls.UI.Submit.performed += SubmitPerformed;

        }

        public void switchActionMap(ActionMap actionMap)
        {
            Debug.Log("Switching to action map: " + actionMap);

            playerInput.SwitchCurrentActionMap(actionMap.ToString());
            if (actionMap == ActionMap.UI)
            {
                playerControls.UI.Enable();
                playerControls.Player.Disable();
            }
            else
            {
                playerControls.UI.Disable();
                playerControls.Player.Enable();
            }

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

        private void InteractPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Interact Performed");
            OnPressInteract?.Invoke();
        }

        private void SprintPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Sprint Performed");
            OnSprintPressed?.Invoke(true);
        }

        private void SprintCanceled(InputAction.CallbackContext context)
        {
            Debug.Log("Sprint Canceled");
            OnSprintPressed?.Invoke(false);
        }

        private void SubmitPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Submit Performed");
            OnSubmit?.Invoke();
        }

        private void FixedUpdate()
        {
            if (movementInput != Vector2.zero)
            {
                OnPressDirection?.Invoke(movementInput);
            }

        }
    }
}