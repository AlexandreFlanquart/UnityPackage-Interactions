using UnityEngine.EventSystems;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    /// <summary>
    /// Condition that checks if the mouse pointer is hovering over this object's UI element.
    /// Uses EventSystem for pointer detection (works with UI Canvas and EventSystem).
    /// 
    /// Workflow:
    /// 1. Listens to pointer enter/exit events from EventSystem
    /// 2. When pointer enters, sets isReady to true and fires onConditionMet(true)
    /// 3. When pointer exits, sets isReady to false and fires onConditionMet(false)
    /// 4. AInteractable can now proceed with interaction when this condition is ready
    /// 
    /// Requirements:
    /// - Physic raycaster on camera (for 3D) or phisyc 2D raycaster (for 2D)
    /// - EventSystem must be present in scene (automatically created in UI Canvas)
    /// </summary>
    public class ConditionMouseHover : ACondition, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>Called when pointer enters on this element (via EventSystem)</summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            isReady = true;
            OnConditionMet(true);
        }
        
        /// <summary>Called when pointer exits on this element (via EventSystem)</summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            isReady = false;
            OnConditionMet(false);
        }
        
        /// <summary>Reset hover state on disable — EventSystem won't fire OnPointerExit on disabled objects</summary>
        private void OnDisable()
        {
            if (isReady)
            {
                isReady = false;
                OnConditionMet(false);
            }
        }

        /// <summary>Returns whether mouse is currently hovering over this element</summary>
        protected override bool EvaluateCondition()
        {
            return isReady;
        }
    }
}
