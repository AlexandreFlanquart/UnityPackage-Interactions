using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Abstract base class for all conditions in the Interaction System.
    /// Conditions are checks that must pass before interactions can be executed.
    /// Examples: player proximity (ConditionRange), mouse hover (ConditionMouseHover), item possession, etc.
    /// 
    /// Workflow:
    /// 1. Derived classes implement condition-specific logic (e.g., checking distance, hover state)
    /// 2. When condition state changes, OnConditionMet() is called to notify subscribers
    /// 3. AInteractable listens to these events via onConditionMet and decides if interaction can proceed
    /// 4. If all required conditions are ready, the interaction can be triggered
    /// </summary>
    public abstract class ACondition : MonoBehaviour
    {
        /// <summary>Determines if this condition should evaluate to true or false (inverts the check if false). Useful for "NOT" logic</summary>
        protected bool shouldBeTrue = true;
        
        /// <summary>If true, this condition is mandatory for effects to execute. If false, it only blocks interaction but not effect</summary>
        public bool requiredForEffects;
        
        /// <summary>Current state of this condition (true = satisfied, false = not satisfied). Updated by derived classes</summary>
        protected bool isReady = false;

        /// <summary>Event fired when this condition's state changes. Parameter: true if condition is now met, false if not met</summary>
        public event Action<bool> onConditionMet;
        
        /// <summary>
        /// Evaluates the condition, applying the shouldBeTrue inversion if needed.
        /// Do NOT override this — override EvaluateCondition() instead.
        /// </summary>
        public bool CheckCondition()
        {
            bool result = EvaluateCondition();
            return shouldBeTrue ? result : !result;
        }

        /// <summary>
        /// Implement the actual condition logic here.
        /// Return true if the condition is naturally satisfied, false otherwise.
        /// The shouldBeTrue flag is applied automatically by CheckCondition().
        /// </summary>
        protected abstract bool EvaluateCondition();

        /// <summary>
        /// Called by derived classes when the condition state changes.
        /// Broadcasts the state change to all subscribers (mainly AInteractable components listening for condition changes).
        /// </summary>
        /// <param name="conditionMet">True if condition is now satisfied, false if not</param>
        protected virtual void OnConditionMet(bool conditionMet)
        {
            onConditionMet?.Invoke(conditionMet);
        }
    }
}