using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class InteractionTriggerZone : AInteractionTrigger
    {
        [SerializeField] private LayerMask layerMask;

        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

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
        }

    }
}