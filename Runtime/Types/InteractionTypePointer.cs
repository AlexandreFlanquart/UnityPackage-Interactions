using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUnityPackage.Interactions
{
    public class InteractionTypePointer : AInteractionType, IPointerClickHandler
    {
        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clic clic");
            onInteract?.Invoke();
        }

    }
}