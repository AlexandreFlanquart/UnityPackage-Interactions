using System;
using System.Collections;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Abstract base class for all interactable objects in the Interaction System.
    /// Manages the complete interaction lifecycle: activation, conditions, triggers, and effects.
    /// 
    /// Workflow:
    /// 1. OnStart or Manual activation: interaction becomes enabled/disabled
    /// 2. Conditions monitored: when all required conditions are met, interaction is ready
    /// 3. Trigger detected: player enters zone, clicks, or collides (depends on trigger type)
    /// 4. Interaction execute: all effects fire (enter, interact, exit) in sequence
    /// 5. Once complete: interaction can repeat or disable (depends on "once" setting)
    /// 
    /// Key Events:
    /// - onEnter: Player can interact (conditions met + player nearby)
    /// - onInteract: Player actively interacts (click, collision, etc.)
    /// - onExit: Player no longer can interact (left zone, condition failed)
    /// 
    /// Setup:
    /// - Optionally Assign a Trigger (IInteractionTrigger: Zone, Pointer, Collider, etc.)
    /// - Optionally Assign Effects to execute (visual, audio, logic feedback)
    /// - Optionally assign Conditions that must pass (proximity, item possession, etc.)
    /// </summary>
    public abstract class AInteractable : MonoBehaviour
    {
        /// <summary>Controls when this interaction becomes active (OnStart = automatic, Manual = via code)</summary>
        [SerializeField] private ActivationType activationType = ActivationType.OnStart;
        
        /// <summary>Delay in seconds before interaction becomes active after scene starts</summary>
        [SerializeField] private float delay = default;
        
        /// <summary>If true, interaction can only be used once then disables. If false, repeats indefinitely</summary>
        [SerializeField] private bool once = true;

        /// <summary>The trigger that detects player actions (zone enter, click, collision, etc.)</summary>
        [SerializeField] protected AInteractionTrigger interactionTrigger;
        
        /// <summary>Array of effects to execute at different interaction stages (enter/interact/exit)</summary>
        [SerializeField] private AEffect[] effects;
        
        /// <summary>Array of conditions that must be satisfied before interaction can proceed</summary>
        [SerializeField] private ACondition[] conditions;

        /// <summary>Current state of interaction (None, onEnterActive, onInteractActive)</summary>
        private CurrentState currentState = CurrentState.None;
        
        /// <summary>Fired when interaction is enabled and ready to use</summary>
        protected event Action onEnableAction;
        
        /// <summary>Fired when interaction is disabled and no longer available</summary>
        protected event Action onDisableAction;
        
        /// <summary>Fired when player enters interaction</summary>
        protected event Action onEnterAction;
        
        /// <summary>Fired when player exits interaction</summary>
        protected event Action onExitAction;
        
        /// <summary>Fired when player actively interacts (click, collision, etc.)</summary>
        protected event Action onInteractAction;

        /// <summary>Is this interaction currently enabled and available to use?</summary>
        protected bool isEnable = false;
        
        /// <summary>Cached value: does this interaction have a trigger assigned?</summary>
        protected bool hasTrigger = false;
        
        /// <summary>Cached value: does this interaction have any conditions?</summary>
        protected bool hasConditions = false;
        
        /// <summary>Prevents multiple simultaneous interactions (interaction in progress)</summary>
        private bool isInteractionRunning = false;

        /// <summary>True after Start() has run — used to guard OnEnable re-subscription</summary>
        private bool hasInitialized = false;

        /// <summary>When should this interaction become active</summary>
        private enum ActivationType { OnStart, OnEnable, Manual }

        /// <summary>Current state of the interaction lifecycle</summary>
        private enum CurrentState { None, onEnterActive, onInteractActive };

        /// <summary>
        /// Checks if all conditions are currently satisfied (for any condition type).
        /// Used to determine if interaction can proceed.
        /// </summary>
        protected bool isConditionsReady;
        
        /// <summary>
        /// Property that evaluates if all conditions are ready.
        /// Returns true if no conditions, or if all conditions pass CheckCondition().
        /// </summary>
        protected bool IsConditionsReady
        {
            get
            {
                if (conditions == null || conditions.Length == 0) return true;
                bool isReady = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] == null) continue;
                    if (!conditions[i].CheckCondition()) isReady = false;
                }
                return isReady;
            }
        }
        
        /// <summary>
        /// stock IsRequiredConditionsActives property that checks if all REQUIRED conditions are satisfied.
        /// </summary>
        protected bool isRequiredConditionsActives;
        
        /// <summary>
        /// Property that evaluates if all REQUIRED conditions are ready.
        /// Returns true if: no conditions, all required conditions pass
        /// if no required conditions exist return isConditionsReady.
        /// Returns false if any required condition fails.
        /// </summary>
        protected bool IsRequiredConditionsActives
        {
            get
            {
                if (conditions == null || conditions.Length == 0) return true;
                bool hasRequiredCondition = false;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] == null) continue;
                    if (conditions[i].requiredForEffects)
                    {
                        if (!conditions[i].CheckCondition())
                            return false;
                        hasRequiredCondition = true;
                    }
                }
                if (!hasRequiredCondition) return IsConditionsReady;
                return true;
            }
        }
        
        /// <summary>
        /// Called by derived classes to initialize interaction-specific logic.
        /// Subscribe to onInteractAction here to define what happens when player interacts.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Initialize interaction system on scene start.
        /// Sets up trigger listeners, condition listeners, and activates if configured.
        /// </summary>
        protected virtual void Start()
        {
            if (activationType == ActivationType.OnStart) StartCoroutine(ActiveAfterDelay());
            Init();

            currentState = CurrentState.None;

            // Cache trigger and condition presence
            hasTrigger = interactionTrigger != null;
            hasConditions = conditions != null && conditions.Length > 0;

            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;

            hasInitialized = true;
            SubscribeEvents();
        }

        /// <summary>
        /// Re-subscribe to events and re-activate when GameObject is re-enabled.
        /// Activates for OnStart and OnEnable types. Manual type requires explicit Enable() call.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (!hasInitialized) return;
            SubscribeEvents();
            if (activationType != ActivationType.Manual) StartCoroutine(ActiveAfterDelay());
        }

        /// <summary>
        /// Cleanup when interaction is disabled.
        /// Fires exit effects if disabled mid-interaction or while player was in zone,
        /// then unsubscribes all events to prevent memory leaks.
        /// </summary>
        protected virtual void OnDisable()
        {
            // If disabled while player was in zone or mid-interaction, fire exit effects cleanly
            if (isEnable && currentState != CurrentState.None)
            {
                if (effects != null)
                {
                    for (int i = 0; i < effects.Length; i++)
                    {
                        if (effects[i] == null) continue;
                        effects[i].ExitEffect();
                    }
                }
                onExitAction?.Invoke();
            }

            currentState = CurrentState.None;
            UnsubscribeEvents();
            isEnable = false;
            isInteractionRunning = false;
        }

        /// <summary>Subscribe to condition and trigger events.</summary>
        private void SubscribeEvents()
        {
            if (conditions != null)
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] == null) continue;
                    conditions[i].onConditionMet += OnConditionChanged;
                }
            }

            if (interactionTrigger != null)
            {
                interactionTrigger.onInteract += OnInteractTrigger;
                interactionTrigger.onEnter += OnEnterTrigger;
                interactionTrigger.onExit += OnExitTrigger;
            }
        }

        /// <summary>Unsubscribe from condition and trigger events.</summary>
        private void UnsubscribeEvents()
        {
            if (conditions != null)
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] == null) continue;
                    conditions[i].onConditionMet -= OnConditionChanged;
                }
            }

            if (interactionTrigger != null)
            {
                interactionTrigger.onInteract -= OnInteractTrigger;
                interactionTrigger.onEnter -= OnEnterTrigger;
                interactionTrigger.onExit -= OnExitTrigger;
            }
        }

        /// <summary>
        /// Called when any condition's state changes.
        /// Determines if interaction should enter/exit/interact states based on current condition status.
        /// 
        /// Logic:
        /// - If conditions become ready AND we're not interacting: Enter state
        /// - If required conditions fail AND we're in enter state: Exit state
        /// - If all conditions ready AND already in enter state AND no trigger: Interact state
        /// </summary>
        /// <param name="conditionMet">True if the condition that changed is now met, false otherwise</param>
        public void OnConditionChanged(bool conditionMet)
        {
            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;


            // If conditions became ready, enter the interaction zone
            if (conditionMet && currentState == CurrentState.None && isRequiredConditionsActives)
            {
                OnEnter();
            }
            // If conditions failed, exit the interaction zone
            else if (!conditionMet && currentState == CurrentState.onEnterActive)
            {
                OnExit();
            }

            // If no trigger exists and all conditions ready, auto-interact (condition-driven interaction)
            if (conditionMet && currentState == CurrentState.onEnterActive && isConditionsReady && !hasTrigger)
            {
                OnInteract();
            }
        }

        /// <summary>
        /// Called when trigger detects interaction (click, collision, etc.).
        /// Only executes if all conditions are satisfied.
        /// </summary>
        private void OnInteractTrigger()
        {
            if (isConditionsReady)
            {
                OnInteract();
            }
        }

        /// <summary>
        /// Called when trigger detects player entering zone.
        /// Subclasses can override to check/display unsatisfied conditions.
        /// </summary>
        protected virtual void OnEnterTrigger()
        {
        }

        /// <summary>
        /// Called when trigger detects player leaving zone.
        /// Subclasses can override for cleanup.
        /// </summary>
        protected virtual void OnExitTrigger()
        {
        }

        /// <summary>
        /// Execute the interaction when player actively interacts.
        /// Fires all InteractEffect() on all effects, marks state as active, and invokes onInteractAction.
        /// </summary>
        public void OnInteract()
        {

            if (!isEnable || isInteractionRunning) return;
            
            isInteractionRunning = true;
            
            // Fire interact effect on all effects
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] == null) continue;
                    effects[i].InteractEffect();
                }
            }

            currentState = CurrentState.onInteractActive;
            onInteractAction?.Invoke();
        }

        /// <summary>
        /// Called when player enters interaction zone and all conditions are satisfied.
        /// Fires EnterEffect() on all effects and marks state as enter-active.
        /// </summary>
        public void OnEnter()
        {
            if (!isEnable || currentState == CurrentState.onEnterActive)
                return;

            // Fire enter effect on all effects
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] == null) continue;
                    effects[i].EnterEffect();
                }
            }
            
            currentState = CurrentState.onEnterActive;
            onEnterAction?.Invoke();
        }

        /// <summary>
        /// Called when player exits interaction zone or conditions no longer met.
        /// Fires ExitEffect() on all effects and resets state.
        /// </summary>
        public void OnExit()
        {

            if (!isEnable) return;
            
            // Fire exit effect on all effects
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] == null) continue;
                    effects[i].ExitEffect();
                }
            }

            currentState = CurrentState.None;
            onExitAction?.Invoke();
        }
        /// <summary>
        /// Coroutine that handles post-interaction cleanup.
        /// Exits interaction state, and either disables (if once=true) or repeats (if once=false).
        /// </summary>
        protected IEnumerator EndInteractionCoroutine()
        {
            currentState = CurrentState.None;
            OnExit();
            yield return null;

            isInteractionRunning = false;

            if (once)
            {
                // One-time interaction: disable after use
                Disable();
            }
            else
            {
                // Repeating interaction: check if should auto-interact or re-enter
                CheckIfAlreadyReady();
            }
        }
        
        /// <summary>
        /// Called by derived classes when interaction completes.
        /// Starts the EndInteractionCoroutine to handle cleanup.
        /// </summary>
        protected void EndInteraction()
        {
            StartCoroutine(EndInteractionCoroutine());
        }

        /// <summary>
        /// Enable this interaction: make it available to the player.
        /// Fires ActivateEffect() on all effects.
        /// </summary>
        public virtual void Enable()
        {
            if (isEnable) return;
            isEnable = true;

            // Fire activate effect on all effects
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] == null) continue;
                    effects[i].ActivateEffect();
                }
            }
            onEnableAction?.Invoke();

            // Defer condition check to next frame so all component OnEnable() calls
            // have completed (RangeHandler re-registers and forces a fresh distance calc)
            StartCoroutine(CheckIfAlreadyReadyNextFrame());
        }

        private IEnumerator CheckIfAlreadyReadyNextFrame()
        {
            yield return null;
            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;
            CheckIfAlreadyReady();
        }

        /// check if condition is already ready
        public void CheckIfAlreadyReady()
        {
            if (isConditionsReady && !hasTrigger)
            {
                OnInteract();
            }
            else if (isRequiredConditionsActives && hasConditions)
            {
                OnEnter();
            }
        }
        /// <summary>
        /// Disable this interaction: make it unavailable to the player.
        /// Fires DeactivateEffect() on all effects.
        /// </summary>
        public virtual void Disable()
        {
            if (!isEnable) return;
            isEnable = false;
            
            // Fire deactivate effect on all effects
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] == null) continue;
                    effects[i].DeactivateEffect();
                }
            }
            onDisableAction?.Invoke();
        }

        /// <summary>
        /// Wait for the configured delay, then enable the interaction.
        /// If delay is 0, Enable() is called synchronously (no frame deferral)
        /// so that trigger re-fires in the same frame find isEnable = true.
        /// </summary>
        private IEnumerator ActiveAfterDelay()
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);
            Enable();
        }
    }
}