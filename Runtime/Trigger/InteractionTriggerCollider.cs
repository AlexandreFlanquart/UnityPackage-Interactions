using UnityEngine;
using System;

namespace MyUnityPackage.Interactions
{
    public class InteractionTriggerCollider : AInteractionTrigger
    {
        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

        [SerializeField] private LayerMask layerMask;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnCollisionEnter(Collision col)
        {
            if (layerMask != (layerMask | (1 << col.gameObject.layer)))
                return;

            onEnter?.Invoke();
            onInteract?.Invoke();
        }
        private void OnCollisionExit(Collision col)
        {
            if (layerMask != (layerMask | (1 << col.gameObject.layer)))
                return;

            onExit?.Invoke();
        }
    }
}