using UnityEngine;

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
            // Subscribe to range handler events
            rangeHandler.onRangeEnter += OnRangeEnter;
            rangeHandler.onRangeExit += OnRangeExit;
        }

        void OnDisable()
        {
            // Unsubscribe from range handler events
            rangeHandler.onRangeEnter -= OnRangeEnter;
            rangeHandler.onRangeExit -= OnRangeExit;
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

        /// <summary>Returns whether player is currently within range</summary>
        public override bool CheckCondition()
        {
            return isReady;
        }
    }
}
