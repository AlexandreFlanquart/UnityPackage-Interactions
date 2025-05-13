using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionTypePointer : AInteractionType, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public override event Action onEnter;
    public override event Action onExit;
    public override event Action onInteract;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onInteract?.Invoke();
    }
}
