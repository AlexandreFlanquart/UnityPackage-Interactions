using UnityEngine;
using UnityEngine.Events;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Effect implementation that invokes UnityEvents.
    /// 
    /// Workflow:
    /// 1. AInteractable calls appropriate effect method based on interaction state
    /// 2. This component invokes the corresponding UnityEvent
    /// 
    /// Setup:
    /// - Assign UnityEvent handlers in Inspector for each lifecycle stage
    /// - Can call methods on any component without writing new code
    /// </summary>
    public class EffectUnityEvent : AEffect
    {
        /// <summary>UnityEvent invoked when interaction becomes enabled/available</summary>
        [SerializeField] private UnityEvent onEnableUEvent;
        
        /// <summary>UnityEvent invoked when interaction becomes disabled/unavailable</summary>
        [SerializeField] private UnityEvent onDisableUEvent;
        
        /// <summary>UnityEvent invoked when conditions for interaction are met (e.g., player enters trigger zone)</summary>
        [SerializeField] private UnityEvent onEnterUEvent;
        
        /// <summary>UnityEvent invoked when conditions for interaction are no longer met (e.g., player exits trigger zone)</summary>
        [SerializeField] private UnityEvent onExitUEvent;
        
        /// <summary>UnityEvent invoked when player actively interacts</summary>
        [SerializeField] private UnityEvent onInteractUEvent;

        /// <summary>Invokes onEnableUEvent</summary>
        public override void ActivateEffect()
        {
            onEnableUEvent?.Invoke();
        }

        /// <summary>Invokes onDisableUEvent</summary>
        public override void DeactivateEffect()
        {
            onDisableUEvent?.Invoke();
        }

        /// <summary>Invokes onEnterUEvent</summary>
        public override void EnterEffect()
        {
            onEnterUEvent?.Invoke();
        }

        /// <summary>Invokes onExitUEvent</summary>
        public override void ExitEffect()
        {
            onExitUEvent?.Invoke();
        }

        /// <summary>Invokes onInteractUEvent</summary>
        public override void InteractEffect()
        {
            onInteractUEvent?.Invoke();
        }
    }
}
