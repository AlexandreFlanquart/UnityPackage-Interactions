using UnityEngine.EventSystems;

namespace MyUnityPackage.Interactions
{
    public class ConditionMouseHover : ACondition, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            isReady = true;
            OnConditionMet(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            isReady = false;
            OnConditionMet(false);
        }
        public override bool CheckCondition()
        {
            return isReady;
        }
    }
}
