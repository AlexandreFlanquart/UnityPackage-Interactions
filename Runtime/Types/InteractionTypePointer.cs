using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUnityPackage.Interactions
{
    public class InteractionTypePointer : AInteractionType, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Clic entré");
            onEnter?.Invoke();

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Clic sorti");
            onExit?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clic clic");
            onInteract?.Invoke();
        }


    }
}