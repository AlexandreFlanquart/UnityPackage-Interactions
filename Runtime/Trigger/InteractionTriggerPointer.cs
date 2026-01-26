using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Trigger that detects pointer/mouse clicks on UI elements.
    /// Fires interaction events when player clicks on objects with this component.
    /// 
    /// Workflow:
    /// 1. Implements IPointerClickHandler from EventSystem
    /// 2. Listens for pointer click events on this UI element
    /// 3. Fires onInteract when player clicks
    /// 4. Note: Does not fire onEnter/onExit (pointer-based, not zone-based)
    /// 
    /// Setup:
    /// - Object must have a GraphicRaycaster (for UI) or Physics2DRaycaster (for 2D world)
    /// - EventSystem must be present in scene (automatic in UI Canvas)
    /// - For 3D world objects, consider using a UI Overlay or use collision-based triggers
    /// 
    /// Requirements:
    /// - Canvas with EventSystem
    /// - GraphicRaycaster on Canvas for UI
    /// - Physics2DRaycaster on Camera for 2D world objects
    /// </summary>
    public class InteractionTriggerPointer : AInteractionTrigger, IPointerClickHandler
    {
        /// <summary>Event fired when player clicks on this object (not used for pointer triggers)</summary>
        public override event Action onEnter;
        
        /// <summary>Event fired when player stops clicking (not used for pointer triggers)</summary>
        public override event Action onExit;
        
        /// <summary>Event fired when player clicks on this object</summary>
        public override event Action onInteract;

        /// <summary>Called by EventSystem when pointer clicks this element</summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            onInteract?.Invoke();
        }
    }
}