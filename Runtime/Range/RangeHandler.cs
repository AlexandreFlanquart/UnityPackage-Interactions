using System;
using UnityEngine;
using MyUnityPackage.Toolkit;
using Unity.Plastic.Antlr3.Runtime.Tree;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Handles proximity detection and distance tracking for a specific interaction point.
    /// Monitors whether the player is within range and broadcasts distance updates.
    /// 
    /// Workflow:
    /// 1. RangeChecker periodically calls CheckRange() with calculated distance
    /// 2. CheckRange() determines if player is in range (distance <= maxDistance)
    /// 3. When state changes (enter/exit range), fires onRangeEnter or onRangeExit
    /// 4. Every frame while in range, fires onPlayerMoveInRange with current distance
    /// 5. Systems like ConditionRange and InteractionIconEffect use these events
    /// 
    /// Setup:
    /// - Add to object you want to track proximity for
    /// - rangeCheckerRegisterType: Auto = registers automatically, Manual = register via script
    /// - Set center (defaults to this object's transform)
    /// - Set maxDistance for interaction range
    /// </summary>
    public class RangeHandler : MonoBehaviour
    {
        /// <summary>Controls how this handler registers with RangeChecker system</summary>
        private enum RangeCheckerRegisterType { Auto, Manual }
        
        /// <summary>Registration mode (Auto = automatic, Manual = code-controlled)</summary>
        [SerializeField] private RangeCheckerRegisterType rangeCheckerRegisterType = RangeCheckerRegisterType.Auto;
        
        /// <summary>Center point for distance calculations (defaults to this transform if null)</summary>
        [SerializeField] private Transform center;
        
        /// <summary>Maximum distance for player to be considered "in range" (in units)</summary>
        [SerializeField] private float maxDistance;

        /// <summary>Fired when player enters the interaction range</summary>
        public event Action onRangeEnter;
        
        /// <summary>Fired when player exits the interaction range</summary>
        public event Action onRangeExit;
        
        /// <summary>Fired every check while player is in range. Parameter: current distance from player</summary>
        public event Action<float> onPlayerMoveInRange;

        /// <summary>Is player currently within range?</summary>
        private bool inRange = false;

        /// <summary>Returns the center point (this transform if center is null)</summary>
        public Transform Center { get => center != null ? center : transform; }
        
        /// <summary>Returns the maximum interaction distance</summary>
        public float MaxDistance { get => maxDistance; }
        
        /// <summary>Returns whether player is currently in range</summary>
        public bool InRange { get => inRange; }

        private RangeChecker rangeChecker;

        /// <summary>Initialize: register automatically if set to Auto mode</summary>
        void Start()
        {
            rangeChecker = ServiceLocator.GetService<RangeChecker>();

            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                RegisterToRangeChecker();
            }
        }
        
        /// <summary>Re-register when enabled (in case disabled/enabled during gameplay)</summary>
        void OnEnable()
        {
            if(rangeChecker == null)return;

            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                RegisterToRangeChecker();
            }
        }

        /// <summary>Unregister when disabled to stop receiving distance checks</summary>
        void OnDisable()
        {
            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                UnregisterFromRangeChecker();
            }
        }

        /// <summary>Register this handler with RangeChecker to start receiving distance updates</summary>
        public void RegisterToRangeChecker()
        {
            rangeChecker.AddRangeElement(this);
        }

        /// <summary>Unregister from RangeChecker to stop receiving distance updates</summary>
        public void UnregisterFromRangeChecker()
        {
            rangeChecker.RemoveRangeElement(this);
        }

        /// <summary>Force RangeChecker to immediately calculate distance (useful for initialization)</summary>
        public void ForceCalculateRange()
        {
            rangeChecker.CalculateRange(this);
        }

        /// <summary>
        /// Called by RangeChecker with calculated distance.
        /// Updates range state and broadcasts appropriate events.
        /// </summary>
        /// <param name="pDistance">Current distance from player to this object's center</param>
        public void CheckRange(float pDistance)
        {
            SetInRange(pDistance <= maxDistance);
            if (inRange)
            {
                // Broadcast distance to listeners (e.g., icon scaling effects)
                onPlayerMoveInRange?.Invoke(pDistance);
            }
        }

        /// <summary>
        /// Sets the range state and fires enter/exit events if state changed.
        /// Called by CheckRange() after distance comparison.
        /// </summary>
        /// <param name="pIsInRange">True if player is now in range, false otherwise</param>
        public void SetInRange(bool pIsInRange)
        {
            if (inRange == pIsInRange) return;

            inRange = pIsInRange;
            if (inRange)
            {
                onRangeEnter?.Invoke();
            }
            else
            {
                onRangeExit?.Invoke();
            }
        }

        #region Utility
#if UNITY_EDITOR
        /// <summary>Color for range visualization gizmo in scene view</summary>
        private Color gizmoColor = Color.cyan; 

        /// <summary>Show interaction range as wireframe sphere in scene editor</summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(Center.position, maxDistance); 
        }
#endif
        #endregion
    }
}
