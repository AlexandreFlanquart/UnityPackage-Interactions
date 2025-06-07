using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUnityPackage.Interactions
{
    public class ConditionMouse : ACondition, IPointerEnterHandler, IPointerExitHandler
    {
        bool isHover;
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Je passe sur l'objet");
            isHover = true;
            OnConditionMet(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Je sort de l'objet");
            isHover = false;
            OnConditionMet(false);
        }
        public override bool CheckCondition()
        {
            return isHover;
        }
    }
}
