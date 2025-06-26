using System;
using System.Collections;
using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class InteractionTriggerZone2D : AInteractionTrigger
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private bool waitingForInput = true;

        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

        private InputManager inputManager;
        private Action interactAction;

        void Start()
        {
            inputManager = InputManager.GetInstance();
            interactAction = () =>
            {
                Debug.Log("Interact");
                onInteract?.Invoke();
            };
        }


        void OnDisable()
        {
            inputManager.OnPressInteract -= interactAction;
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OnTriggerEnter2D");
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            if (!waitingForInput)
            {
                Debug.Log("Trigger Enter");
                onInteract?.Invoke();
            }
            else
            {
                onEnter?.Invoke();
                inputManager.OnPressInteract += interactAction;
            }

        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            onExit?.Invoke();
            StopAllCoroutines();
            inputManager.OnPressInteract -= interactAction;
        }

    }
}
