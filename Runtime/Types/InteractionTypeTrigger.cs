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
        private Action interactAction;

        void Start()
        {
            interactAction = () => onInteract?.Invoke();
        }


        void OnTriggerEnter(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;

            onEnter?.Invoke();
            onInteract?.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;

            onExit?.Invoke();
            StopAllCoroutines();
        }

    }
}