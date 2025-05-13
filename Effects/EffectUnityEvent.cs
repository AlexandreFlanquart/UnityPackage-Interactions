using UnityEngine;
using UnityEngine.Events;
public class EffectUnityEvent : AEffect
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    [SerializeField] private UnityEvent onInteract;

    public override void OnEnter()
    {
        onEnter?.Invoke();
    }

    public override void OnExit()
    {
        onExit?.Invoke();
    }

    public override void OnInteract()
    {
        onInteract?.Invoke();
    }
}
