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

        /// <summary>
        /// Full interaction lifecycle. Single source of truth — replaces the previous
        /// isEnable/currentState/isInteractionRunning flag trio, whose independent updates
        /// were the root cause of several state-desync bugs.
        /// </summary>
        private enum LifecycleState
        {
            /// <summary>Not available to the player (initial state, after Disable(), or used up when once=true)</summary>
            Disabled,
            /// <summary>Enabled and idle — waiting for the player to enter / conditions to be met</summary>
            Ready,
            /// <summary>Player entered (zone/conditions) — EnterEffect fired, waiting for interact</summary>
            Entered,
            /// <summary>Interaction running — waiting for the subclass to call EndInteraction()</summary>
            Interacting,
            /// <summary>One-frame cooldown at the end of an interaction, before the once/repeat decision</summary>
            WindingDown
        }

        /// <summary>Current lifecycle state. Only mutated by the public lifecycle methods below.</summary>
        private LifecycleState state = LifecycleState.Disabled;

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
        protected bool isEnable => state != LifecycleState.Disabled;

        /// <summary>Cached value: does this interaction have a trigger assigned?</summary>
        protected bool hasTrigger = false;

        /// <summary>Cached value: does this interaction have any conditions?</summary>
        protected bool hasConditions = false;

        /// <summary>True once a "once" interaction has completed — prevents Enable() from re-arming it (e.g. via a GameObject disable/enable cycle)</summary>
        private bool hasCompletedOnce = false;

        /// <summary>True after Start() has run — used to guard OnEnable re-subscription</summary>
        private bool hasInitialized = false;

        /// <summary>When should this interaction become active</summary>
        private enum ActivationType { OnStart, OnEnable, Manual }

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
            Init();

            state = LifecycleState.Disabled;

            // Cache trigger and condition presence
            hasTrigger = interactionTrigger != null;
            hasConditions = conditions != null && conditions.Length > 0;

            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;

            hasInitialized = true;
            SubscribeEvents();

            // Activation happens last: Enable() can run synchronously (delay == 0), so
            // everything it may rely on (Init()'s subscriptions, SubscribeEvents()) must
            // already be in place before it fires.
            if (activationType == ActivationType.OnStart) StartCoroutine(ActiveAfterDelay());
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
        /// Note: intentionally does NOT fire DeactivateEffect/onDisableAction — that pair
        /// belongs to the explicit Disable() API, not the Unity lifecycle.
        /// </summary>
        protected virtual void OnDisable()
        {
            // If disabled while player was in zone or mid-interaction, fire exit effects cleanly
            if (state == LifecycleState.Entered || state == LifecycleState.Interacting)
            {
                FireEffects(e => e.ExitEffect());
                onExitAction?.Invoke();
            }

            state = LifecycleState.Disabled;
            UnsubscribeEvents();
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
        /// - If conditions become ready AND we're idle: Enter state
        /// - If required conditions fail AND we're in enter state: Exit state
        /// - If all conditions ready AND already in enter state AND no trigger: Interact state
        /// </summary>
        /// <param name="conditionMet">True if the condition that changed is now met, false otherwise</param>
        public void OnConditionChanged(bool conditionMet)
        {
            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;

            // If conditions became ready, enter the interaction zone
            if (conditionMet && state == LifecycleState.Ready && isRequiredConditionsActives)
            {
                OnEnter();
            }
            // If conditions failed, exit the interaction zone
            else if (!conditionMet && state == LifecycleState.Entered)
            {
                OnExit();
            }

            // If no trigger exists and all conditions ready, auto-interact (condition-driven interaction)
            if (conditionMet && state == LifecycleState.Entered && isConditionsReady && !hasTrigger)
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
        /// Fires OnEnter() (gated by required conditions, same as the condition-driven path)
        /// so a trigger-only interactable (no Condition assigned) still gets EnterEffect().
        /// Subclasses can override to add extra behavior (e.g. display unsatisfied conditions).
        /// </summary>
        protected virtual void OnEnterTrigger()
        {
            if (isRequiredConditionsActives) OnEnter();
        }

        /// <summary>
        /// Called when trigger detects player leaving zone.
        /// Fires OnExit() only if currently in the entered state, mirroring the condition-driven path.
        /// Subclasses can override for extra cleanup.
        /// </summary>
        protected virtual void OnExitTrigger()
        {
            if (state == LifecycleState.Entered) OnExit();
        }

        /// <summary>
        /// Execute the interaction when player actively interacts.
        /// Fires all InteractEffect() on all effects, marks state as active, and invokes onInteractAction.
        /// Ignored unless the interaction is Ready or Entered with all conditions satisfied.
        /// </summary>
        public void OnInteract()
        {
            if (state != LifecycleState.Ready && state != LifecycleState.Entered) return;
            if (!isConditionsReady) return;

            // State first: acts as the re-entrancy lock for the whole method.
            state = LifecycleState.Interacting;

            // Mark "once" interactions as used up immediately — a synchronous handler could
            // deactivate this GameObject before EndInteractionCoroutine finishes (Unity kills
            // the coroutine on deactivation), which would otherwise let the interaction re-arm.
            if (once) hasCompletedOnce = true;

            FireEffects(e => e.InteractEffect());

            // An effect may have reentrantly changed the state (e.g. EffectUnityEvent wired
            // to Disable()) — don't resurrect it, and don't run the action.
            if (state != LifecycleState.Interacting) return;

            onInteractAction?.Invoke();
        }

        /// <summary>
        /// Called when player enters interaction zone and all conditions are satisfied.
        /// Fires EnterEffect() on all effects and marks state as entered.
        /// Ignored unless the interaction is Ready (blocks re-entry mid-interaction and
        /// during the end-of-interaction wind-down frame).
        /// </summary>
        public void OnEnter()
        {
            if (state != LifecycleState.Ready) return;

            state = LifecycleState.Entered;
            FireEffects(e => e.EnterEffect());

            // An effect may have reentrantly changed the state — don't resurrect it.
            if (state != LifecycleState.Entered) return;

            onEnterAction?.Invoke();
        }

        /// <summary>
        /// Called when player exits interaction zone or conditions no longer met.
        /// Fires ExitEffect() on all effects and returns to Ready.
        /// Ignored unless the interaction is Entered or Interacting (no phantom exits).
        /// </summary>
        public void OnExit()
        {
            if (state != LifecycleState.Entered && state != LifecycleState.Interacting) return;

            state = LifecycleState.Ready;
            FireEffects(e => e.ExitEffect());

            // An effect may have reentrantly changed the state — don't resurrect it.
            if (state != LifecycleState.Ready) return;

            onExitAction?.Invoke();
        }

        /// <summary>
        /// Coroutine that handles post-interaction cleanup.
        /// Exits interaction state, holds a one-frame wind-down (so trigger/condition events
        /// can't re-enter before the once/repeat decision), then either disables (once=true)
        /// or re-arms (once=false). No-ops if no interaction is actually running.
        /// </summary>
        protected IEnumerator EndInteractionCoroutine()
        {
            if (state != LifecycleState.Interacting) yield break;

            OnExit(); // Interacting -> Ready, fires ExitEffect + onExitAction

            // Hold the wind-down state for one frame — blocks OnEnter/OnInteract until the
            // once/repeat decision below (a handler of onExitAction may already have changed
            // the state, e.g. a reentrant Disable(): respect it).
            if (state == LifecycleState.Ready) state = LifecycleState.WindingDown;
            yield return null;
            if (state == LifecycleState.WindingDown) state = LifecycleState.Ready;

            if (state == LifecycleState.Disabled) yield break; // externally disabled meanwhile

            if (once)
            {
                // One-time interaction: disable after use (hasCompletedOnce was set in OnInteract())
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
        /// Ignored if already enabled, or if a "once" interaction has been used up
        /// (call ResetInteractionState() first to re-arm it).
        /// </summary>
        public virtual void Enable()
        {
            if (once && hasCompletedOnce) return;
            if (state != LifecycleState.Disabled) return;

            state = LifecycleState.Ready;
            FireEffects(e => e.ActivateEffect());

            // An effect may have reentrantly changed the state — don't resurrect it.
            if (state != LifecycleState.Ready) return;

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
        /// Fires exit effects first if currently entered/mid-interaction (mirrors OnDisable()),
        /// then DeactivateEffect() on all effects. Safe to call from any state.
        /// </summary>
        public virtual void Disable()
        {
            // Fire exit effects first if disabling while entered/mid-interaction,
            // so visuals/UI tied to EnterEffect are cleanly turned off.
            if (state == LifecycleState.Entered || state == LifecycleState.Interacting)
            {
                OnExit();
            }

            if (state == LifecycleState.Disabled) return;
            state = LifecycleState.Disabled;

            FireEffects(e => e.DeactivateEffect());
            onDisableAction?.Invoke();
        }

        /// <summary>
        /// Clears the "used up" state of a "once" interaction, allowing it to be enabled again.
        /// Call this only while the interaction is at rest (already disabled) — e.g. before
        /// reactivating a pooled/respawned interactable.
        /// </summary>
        public virtual void ResetInteractionState()
        {
            hasCompletedOnce = false;
        }

        /// <summary>
        /// Wait for the configured delay, then enable the interaction.
        /// If delay is 0, Enable() is called synchronously (no frame deferral)
        /// so that trigger re-fires in the same frame find the interaction enabled.
        /// </summary>
        private IEnumerator ActiveAfterDelay()
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);
            Enable();
        }

        /// <summary>Invokes the given effect method on every assigned (non-null) effect.</summary>
        private void FireEffects(Action<AEffect> fire)
        {
            if (effects == null) return;
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] == null) continue;
                fire(effects[i]);
            }
        }
    }
}
