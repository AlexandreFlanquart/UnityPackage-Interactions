using UnityEngine;
using System;
using MyUnityPackage.Interactions;

namespace MyUnityPackage.Interactions.Samples
{
    public class InteractionTriggerKeyboard : AInteractionTrigger
{
    public override event Action onEnter;
    public override event Action onExit;
    public override event Action onInteract;

    void Start()
    {
        InputManager.GetInstance().OnPressInteract += OnInteract;
    }
    void OnInteract()
    {
        onInteract?.Invoke();
    }
}
}
