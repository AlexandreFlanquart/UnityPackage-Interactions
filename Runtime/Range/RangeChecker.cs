using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUnityPackage.Toolkit;    

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Central manager for distance/range calculations in the Interaction System.
    /// Periodically calculates distances between player and all RangeHandler objects.
    /// 
    /// Workflow:
    /// 1. RangeHandler objects register themselves with AddRangeElement()
    /// 2. RangeChecker runs a coroutine that periodically checks distances
    /// 3. For each RangeHandler, calculates distance and calls CheckRange()
    /// 4. Only recalculates when player position changes (optimization)
    /// 
    /// Optimization:
    /// - Only updates when player actually moves (currentPlayerPos != player.position)
    /// - Configurable check frequency (checkFrequency) to balance performance vs responsiveness
    /// - Can force immediate check with forceCheck = true or ForceCalculateRange()
    /// 
    /// Setup:
    /// - Add to an empty GameObject in your scene
    /// - Assign the player transform
    /// - Adjust checkFrequency if needed (0.1s = 10 checks per second by default)
    /// </summary>
    public class RangeChecker : MonoBehaviour
    {    
        /// <summary>Transform of the player/entity to track distance from</summary>
        [SerializeField] private Transform player;
        
        /// <summary>How often to check distances (in seconds). Lower = more responsive but more CPU usage</summary>
        [SerializeField] private float checkFrequency = 0.1f;

        /// <summary>Cached player position from last check (used to detect movement)</summary>
        private Vector3 currentPlayerPos;
        
        /// <summary>List of all registered RangeHandler objects to check distances for</summary>
        private List<RangeHandler> rangeHandlers = new List<RangeHandler>();

        /// <summary>Force next check even if player hasn't moved (useful for initialization)</summary>
        public bool forceCheck = false;

        void Awake()
        {
            ServiceLocator.AddService<RangeChecker>(gameObject, true);
        }

        /// <summary>Begin distance checking coroutine on scene start</summary>
        void Start()
        {
            StartCalculating();
        }

        /// <summary>Start the periodic distance checking coroutine</summary>
        public void StartCalculating()
        {
            StartCoroutine(Calculating());
        }

        /// <summary>Stop the periodic distance checking coroutine</summary>
        public void StopCalculating()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Coroutine that periodically checks all RangeHandler distances.
        /// Runs continuously at the configured checkFrequency interval.
        /// </summary>
        private IEnumerator Calculating()
        {
            while (true)
            {
                CalculateRange();
                yield return new WaitForSeconds(checkFrequency);
            }
        }

        /// <summary>
        /// Register a RangeHandler to be monitored for distance changes.
        /// Also calculates its distance immediately.
        /// </summary>
        /// <param name="rangeHandler">The RangeHandler component to add to monitoring list</param>
        public void AddRangeElement(RangeHandler rangeHandler)
        {
            if (!rangeHandlers.Contains(rangeHandler))
            {
                rangeHandlers.Add(rangeHandler);
                CalculateRange(rangeHandler);
            }
        }

        /// <summary>
        /// Unregister a RangeHandler from being monitored.
        /// Called when RangeHandler is disabled or destroyed.
        /// </summary>
        /// <param name="rangeHandler">The RangeHandler component to remove from monitoring list</param>
        public void RemoveRangeElement(RangeHandler rangeHandler)
        {
            if (rangeHandlers.Contains(rangeHandler))
            {
                rangeHandlers.Remove(rangeHandler);
            }
        }

        /// <summary>
        /// Calculate distances for all registered RangeHandler objects.
        /// Only updates if player has moved since last check (optimization).
        /// Can be forced with forceCheck flag.
        /// </summary>
        private void CalculateRange()
        {
            // Skip if player is not active in scene
            if (!player.gameObject.activeInHierarchy) return;
            
            // Only recalculate if player moved or force flag is set
            if ((player != null && currentPlayerPos != player.position) || forceCheck)
            {
                forceCheck = false;
                // Update all registered range handlers
                for (int i = 0; i < rangeHandlers.Count; i++)
                {
                    CalculateRange(rangeHandlers[i]);
                }
            }
        }

        /// <summary>
        /// Calculate distance for a specific RangeHandler and update its range state.
        /// Called by CalculateRange() for each registered handler.
        /// </summary>
        /// <param name="pRangeHandler">The RangeHandler to calculate distance for</param>
        public void CalculateRange(RangeHandler pRangeHandler)
        {
            // Cache current player position
            currentPlayerPos = player.position;
            
            // Calculate distance between player and this handler's center
            float dist = Vector3.Distance(player.position, pRangeHandler.Center.position);
            
            // Notify the handler of the current distance
            pRangeHandler.CheckRange(dist);
        }
    }
}