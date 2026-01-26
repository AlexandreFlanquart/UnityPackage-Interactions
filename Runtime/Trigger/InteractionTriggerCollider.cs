using UnityEngine;
using System;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Trigger that detects physical 3D collisions with the player.
    /// Fires interaction events when player collides with this object (using 3D physics colliders).
    /// 
    /// Workflow:
    /// 1. Listens to OnCollisionEnter/OnCollisionExit events (3D physics)
    /// 2. Checks if colliding object is on the specified layerMask
    /// 3. Fires onEnter and onInteract when collision detected
    /// 4. Fires onExit when collision stops
    /// 
    /// Setup:
    /// - Requires Collider (not trigger) on this object and player
    /// - Both objects need Rigidbody (this can be kinematic)
    /// - Set layerMask to match player's layer
    /// - Use for 3D gameplay interactions (pushing, running into NPCs, etc.)
    /// 
    /// Note: For trigger zones, use InteractionTriggerZone instead (with isTrigger = true)
    /// </summary>
    public class InteractionTriggerCollider : AInteractionTrigger
    {
        /// <summary>Event fired when player collides with this object</summary>
        public override event Action onEnter;
        
        /// <summary>Event fired when collision with player stops</summary>
        public override event Action onExit;
        
        /// <summary>Event fired on collision (same as onEnter for non-trigger collisions)</summary>
        public override event Action onInteract;

        /// <summary>LayerMask to filter which objects can trigger this interaction (typically "Player" layer)</summary>
        [SerializeField] private LayerMask layerMask;

        /// <summary>Called by physics when this collider collides with another collider</summary>
        public void OnCollisionEnter(Collision col)
        {
            // Check if colliding object is on the correct layer
            if (layerMask != (layerMask | (1 << col.gameObject.layer)))
                return;

            onEnter?.Invoke();
            onInteract?.Invoke();
        }
        
        /// <summary>Called by physics when collision with another collider stops</summary>
        private void OnCollisionExit(Collision col)
        {
            // Check if colliding object is on the correct layer
            if (layerMask != (layerMask | (1 << col.gameObject.layer)))
                return;

            onExit?.Invoke();
        }
    }
}