using UnityEngine;
using System;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class InteractionTypeKeyboard : AInteractionType
    {
        public override event Action onEnter;
        public override event Action onExit;
        public override event Action onInteract;

        void Start()
        {
            ServiceLocator.GetService<InputManager>().OnPressInteract += OnInteract;
        }
        void OnInteract()
        {
            Debug.Log("Interact keyboard");
            onInteract?.Invoke();
        }

    }
}
