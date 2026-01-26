using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Simple interaction that executes a UnityEvent and waits for a delay before completing.
    /// Useful for time-delayed interactions (door opening, chest opening, NPC dialogue, etc.).
    /// 
    /// Workflow:
    /// 1. AInteractable triggers this component's onInteractAction
    /// 2. StartSimpleAction() is called
    /// 3. OnInteractionStarted UnityEvent is invoked (for animations, sounds, etc.)
    /// 4. Waits for actionDelay seconds
    /// 5. Calls EndInteraction() to complete the interaction
    /// 
    /// Setup:
    /// - Configure OnInteractionStarted event with desired callbacks
    /// - Set actionDelay to control how long the action takes to complete
    /// - Add this to an object with AInteractable component
    /// </summary>
    public class SimpleAction : AInteractable
    {
        /// <summary>UnityEvent invoked when player interacts with this object</summary>
        [SerializeField] protected UnityEvent OnInteractionStarted = default;
        
        /// <summary>How long to wait (in seconds) before the action completes. Controls interaction duration</summary>
        [SerializeField] protected float actionDelay = 1f;
       
        /// <summary>Called by AInteractable during initialization. Sets up interaction callback</summary>
        protected override void Init()
        {
            // Subscribe to interaction trigger
            onInteractAction += StartSimpleAction;
        }

        /// <summary>Called when player interacts. Invokes the action event and starts the delay timer</summary>
        public void StartSimpleAction()
        {
            if (gameObject.activeInHierarchy)
            {
                OnInteractionStarted?.Invoke();
                StartCoroutine(WaitDelay(actionDelay));
            }
        }

        /// <summary>
        /// Waits for the specified delay, then ends the interaction.
        /// Override this or use actionDelay to control interaction duration.
        /// </summary>
        /// <param name="delay">How many seconds to wait before completing</param>
        private IEnumerator WaitDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EndInteraction();
        }
    }
}