using UnityEngine;
using UnityEngine.EventSystems;
public class ConditionMouse : ACondition, IPointerEnterHandler, IPointerExitHandler
{
    bool isHover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Je passe sur l'objet");
        isHover = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Je sort de l'objet");
        isHover = false;
    }
    public override bool CheckCondition()
    {
        return isHover;
    }
}
