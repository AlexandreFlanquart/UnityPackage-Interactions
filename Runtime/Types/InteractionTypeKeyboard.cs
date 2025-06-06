using UnityEngine;
using System;
using MyUnityPackage.Toolkit;
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
        onInteract?.Invoke();
    }

}
