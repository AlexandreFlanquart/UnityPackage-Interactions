using UnityEngine;

namespace MyUnityPackage.Interactions.Samples
{
    /// <summary>
    /// Effect that dynamically scales and fades an interaction icon based on player distance.
    /// Provides visual feedback showing how close the player is to the interaction point.
    /// 
    /// Workflow:
    /// 1. Listens to RangeHandler distance updates via onPlayerMoveInRange
    /// 2. Calculates alpha (opacity) based on distance: closer = more opaque
    /// 3. Scales icon based on alpha: closer = larger
    /// 4. Shows icon when entering range, hides when exiting
    /// 
    /// Visual Behavior:
    /// - Far from interaction: Icon is small and transparent
    /// - Close to interaction: Icon is large and opaque
    /// - At interaction point: Icon at full size and opacity
    /// 
    /// Requirements:
    /// - SpriteRenderer on this object or child
    /// - RangeHandler component for distance monitoring
    /// - scaleMultiplier controls how much the icon scales (default: 2x)
    /// </summary>
    public class InteractionIconEffect : MonoBehaviour
    {
        /// <summary>SpriteRenderer to display and animate the interaction icon</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        /// <summary>RangeHandler that monitors distance between player and this object</summary>
        [SerializeField] private RangeHandler rangeHandler;
        
        /// <summary>Controls how much the icon scales based on proximity (0-1 alpha multiplier)</summary>
        [SerializeField] private float scaleMultiplier = 2f;

        /// <summary>Whether the interaction can currently be executed (used to determine visual state)</summary>
        public bool canInteract = false;

        /// <summary>Initialize by subscribing to range handler events and forcing initial distance check</summary>
        void Start()
        {
            rangeHandler.onPlayerMoveInRange += OnPlayerMoveInRange;
            rangeHandler.onRangeEnter += OnRangeEnter;
            rangeHandler.onRangeExit += OnRangeExit;
            rangeHandler.ForceCalculateRange();
        }

        /// <summary>
        /// Called every frame when player moves within range.
        /// Updates icon opacity and scale based on distance.
        /// </summary>
        /// <param name="pDistance">Current distance from player to this object</param>
        private void OnPlayerMoveInRange(float pDistance)
        {
            // Calculate alpha: 1.0 at zero distance, 0.0 at max distance
            float alpha = 1f;
            if (rangeHandler.InRange && !canInteract) 
                alpha = 1f - (pDistance / rangeHandler.MaxDistance);
            
            // Apply alpha to sprite color (fade effect)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            
            // Scale icon based on proximity (larger when closer)
            spriteRenderer.transform.localScale = Vector3.one * (1f + alpha * scaleMultiplier);
        }

        /// <summary>Called when player enters the interaction range - show the icon</summary>
        private void OnRangeEnter()
        {
            Show();
        }

        /// <summary>Called when player exits the interaction range - hide the icon</summary>
        private void OnRangeExit()
        {
            Hide();
        }

        /// <summary>Shows the interaction icon</summary>
        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        /// <summary>Hides the interaction icon</summary>
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}