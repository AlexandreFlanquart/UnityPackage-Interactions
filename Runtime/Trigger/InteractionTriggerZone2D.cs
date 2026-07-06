using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Trigger that detects when objects enter/exit a 2D trigger zone.
    /// Fires interaction events when player enters/exits the collider (using 2D physics).
    /// 
    /// Workflow:
    /// 1. Requires a Collider2D with "Is Trigger" enabled (2D trigger zone)
    /// 2. Listens to OnTriggerEnter2D/OnTriggerExit2D physics events
    /// 3. Checks if entering object is on the specified layerMask
    /// 4. Fires onEnter and onInteract when player enters zone
    /// 5. Fires onExit when player leaves zone
    /// 
    /// Setup:
    /// - Collider2D must have "Is Trigger" = true
    /// - Player object must have a non-trigger Collider2D and Rigidbody2D
    /// - Set layerMask to match player's layer
    /// - Use for 2D games with proximity-based interactions
    /// 
    /// Note: For 3D games, use InteractionTriggerZone instead
    /// Note: For collision-based (non-trigger) interactions, use InteractionTriggerCollider (3D only)
    /// </summary>
    public class InteractionTriggerZone2D : AInteractionTrigger
    {
        /// <summary>LayerMask to filter which objects can trigger interactions (typically "Player" layer)</summary>
        [SerializeField] private LayerMask layerMask;

        /// <summary>Event fired when player enters this trigger zone</summary>
        public override event Action onEnter;
        
        /// <summary>Event fired when player exits this trigger zone</summary>
        public override event Action onExit;
        
        /// <summary>Event fired on trigger enter (same as onEnter for zone triggers)</summary>
        public override event Action onInteract;

        /// <summary>Called by 2D physics when an object enters this trigger zone</summary>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, layerMask)) return;
            onEnter?.Invoke();
            onInteract?.Invoke();
        }

        /// <summary>Called by 2D physics when an object exits this trigger zone</summary>
        void OnTriggerExit2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, layerMask)) return;
            onExit?.Invoke();
        }
    }
}
