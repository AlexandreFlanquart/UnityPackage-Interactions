using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Trigger that detects when objects enter/exit a 3D trigger zone.
    /// Fires interaction events when player enters/exits the collider (using 3D physics).
    /// 
    /// Workflow:
    /// 1. Requires a Collider with "Is Trigger" enabled (3D trigger zone)
    /// 2. Listens to OnTriggerEnter/OnTriggerExit physics events
    /// 3. Checks if entering object is on the specified layerMask
    /// 4. Fires onEnter and onInteract when player enters zone
    /// 5. Fires onExit when player leaves zone
    /// 
    /// Setup:
    /// - Collider must have "Is Trigger" = true
    /// - Player object must have a non-trigger Collider and Rigidbody
    /// - Set layerMask to match player's layer
    /// - Use for 3D games with proximity-based interactions
    /// 
    /// Note: For 2D games, use InteractionTriggerZone2D instead
    /// Note: For collision-based (non-trigger) interactions, use InteractionTriggerCollider
    /// </summary>
    public class InteractionTriggerZone : AInteractionTrigger
    {
        /// <summary>LayerMask to filter which objects can trigger interactions (typically "Player" layer)</summary>
        [SerializeField] private LayerMask layerMask;

        /// <summary>Event fired when player enters this trigger zone</summary>
        public override event Action onEnter;
        
        /// <summary>Event fired when player exits this trigger zone</summary>
        public override event Action onExit;
        
        /// <summary>Event fired on trigger enter (same as onEnter for zone triggers)</summary>
        public override event Action onInteract;

        /// <summary>Called by physics when an object enters this trigger zone</summary>
        void OnTriggerEnter(Collider other)
        {
            // Check if entering object is on the correct layer
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;

            onEnter?.Invoke();
            onInteract?.Invoke();
        }

        /// <summary>Called by physics when an object exits this trigger zone</summary>
        void OnTriggerExit(Collider other)
        {
            // Check if exiting object is on the correct layer
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;

            onExit?.Invoke();
        }
    }
}