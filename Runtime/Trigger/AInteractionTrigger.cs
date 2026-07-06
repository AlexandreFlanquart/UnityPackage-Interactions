using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Abstract base class for all interaction triggers in the Interaction System.
    /// Triggers detect when the player or entity performs actions (entering zone, clicking, colliding, etc.).
    /// 
    /// Workflow:
    /// 1. Derived classes listen for player actions (e.g., OnTriggerEnter, OnPointerClick)
    /// 2. When an action occurs, the appropriate event is invoked (onEnter, onExit, onInteract)
    /// 3. AInteractable components subscribe to these events and execute their effects/conditions
    /// 
    /// Event Types:
    /// - onEnter: Fired when player/entity enters the trigger zone or becomes available for interaction
    /// - onExit: Fired when player/entity leaves the trigger zone or interaction becomes unavailable
    /// - onInteract: Fired when player/entity actively interacts (click, collision, etc.)
    /// </summary>
    public abstract class AInteractionTrigger : MonoBehaviour
    {
        /// <summary>Invoked when player enters trigger area or becomes eligible for interaction</summary>
        public abstract event Action onEnter;

        /// <summary>Invoked when player exits trigger area or is no longer eligible for interaction</summary>
        public abstract event Action onExit;

        /// <summary>Invoked when player actively interacts with the object (click, collision, etc.)</summary>
        public abstract event Action onInteract;

        /// <summary>Returns true if the GameObject's layer is included in the given mask. Shared filter for physics-based triggers.</summary>
        protected static bool IsInLayerMask(GameObject go, LayerMask mask)
        {
            return (mask.value & (1 << go.layer)) != 0;
        }
    }
}

