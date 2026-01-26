using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Abstract base class for all effects in the Interaction System.
    /// Effects are visual/audio/gameplay responses that occur during interaction lifecycle.
    /// 
    /// Workflow:
    /// 1. AInteractable manages when effects are activated/deactivated
    /// 2. Trigger events (onEnter/onExit/onInteract) invoke corresponding effect methods
    /// 3. Derived classes implement specific effects (UI updates, sounds, animations, etc.)
    /// 
    /// </summary>
    public abstract class AEffect : MonoBehaviour
    {
        /// <summary>Called when the interaction becomes active/enabled. Use for initialization or showing UI</summary>
        public abstract void ActivateEffect();
        
        /// <summary>Called when the interaction becomes inactive/disabled. Use for cleanup or hiding UI</summary>
        public abstract void DeactivateEffect();
        
        /// <summary>Called when conditions for interaction are met (e.g., player enters trigger zone). Use for entrance animations/sounds</summary>
        public abstract void EnterEffect();
        
        /// <summary>Called when conditions for interaction are no longer met (e.g., player exits trigger zone). Use for exit animations/sounds</summary>
        public abstract void ExitEffect();
        
        /// <summary>Called when player actively interacts (click, collision, etc.). Use for interaction feedback</summary>
        public abstract void InteractEffect();
    }
}
