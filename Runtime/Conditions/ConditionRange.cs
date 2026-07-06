using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Condition that checks if the player is within a certain range of this object.
    /// Uses RangeHandler component to detect proximity and monitor distance changes.
    /// 
    /// Workflow:
    /// 1. Subscribes to RangeHandler events (onRangeEnter, onRangeExit)
    /// 2. When player enters range: sets isReady to true, fires onConditionMet(true)
    /// 3. When player exits range: sets isReady to false, fires onConditionMet(false)
    /// 4. AInteractable uses this to enable/disable interaction based on player proximity
    /// 
    /// Setup:
    /// - Assign a RangeHandler component to the rangeHandler field
    /// - RangeHandler manages actual distance calculations and proximity detection
    /// </summary>
    public class ConditionRange : ACondition
    {
        /// <summary>Reference to the RangeHandler that monitors distance to player</summary>
        [SerializeField] private RangeHandler rangeHandler;

        void OnEnable()
        {
            if (rangeHandler == null)
            {
                MUPLogger.Warning($"ConditionRange on '{gameObject.name}': no RangeHandler assigned — condition will never be met.", this);
                return;
            }

            rangeHandler.onRangeEnter += OnRangeEnter;
            rangeHandler.onRangeExit += OnRangeExit;

            // Sync with current state and notify — RangeHandler may have updated while we were disabled
            bool currentInRange = rangeHandler.InRange;
            if (currentInRange != isReady)
            {
                isReady = currentInRange;
                OnConditionMet(isReady);
            }
        }

        void OnDisable()
        {
            if (rangeHandler == null) return;

            // Unsubscribe from range handler events
            rangeHandler.onRangeEnter -= OnRangeEnter;
            rangeHandler.onRangeExit -= OnRangeExit;

            // Reset state on disable — the RangeHandler's own onRangeExit won't reach us anymore
            // once unsubscribed above, so we notify listeners ourselves (mirrors ConditionMouseHover).
            ResetReadyState();
        }

        /// <summary>Called when player enters the interaction range</summary>
        private void OnRangeEnter()
        {
            isReady = true;
            OnConditionMet(isReady);
        }

        /// <summary>Called when player exits the interaction range</summary>
        private void OnRangeExit()
        {
            isReady = false;
            OnConditionMet(isReady);
        }

        /// <summary>Returns whether player is currently within range, read directly from RangeHandler</summary>
        protected override bool EvaluateCondition()
        {
            return rangeHandler != null && rangeHandler.InRange;
        }
    }
}
