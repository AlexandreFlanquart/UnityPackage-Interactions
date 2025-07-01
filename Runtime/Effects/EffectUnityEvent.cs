using UnityEngine;
using UnityEngine.Events;

namespace MyUnityPackage.Interactions
{
    public class EffectUnityEvent : AEffect
    {
        [SerializeField] private UnityEvent onEnableUEvent;
        [SerializeField] private UnityEvent onDisableUEvent;
        [SerializeField] private UnityEvent onEnterUEvent;
        [SerializeField] private UnityEvent onExitUEvent;
        [SerializeField] private UnityEvent onInteractUEvent;

        public override void OnEnable()
        {
            onEnableUEvent?.Invoke();
        }

        public override void OnDisable()
        {
            onDisableUEvent?.Invoke();
        }

        public override void OnEnter()
        {
            onEnterUEvent?.Invoke();
        }

        public override void OnExit()
        {
            onExitUEvent?.Invoke();
        }

        public override void OnInteract()
        {
            onInteractUEvent?.Invoke();
        }
    }
}
