using System;
using System.Collections;
using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class InteractionTypeTrigger : AInteractionType
    {
        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private bool waitingForInput = false;

        private InputManager inputManager;
        private Action interactAction;

        void Start()
        {
            inputManager = ServiceLocator.GetService<InputManager>();
            interactAction = () => onInteract?.Invoke();
        }

        void OnDisable()
        {
            inputManager.OnPressInteract -= interactAction;
        }

        void OnTriggerEnter(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;

            if (!waitingForInput)
            {
                onInteract?.Invoke();
            }
            else
            {
                onEnter?.Invoke();
                inputManager.OnPressInteract += interactAction;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            onExit?.Invoke();
            StopAllCoroutines();
            inputManager.OnPressInteract -= interactAction;
        }

    }
}