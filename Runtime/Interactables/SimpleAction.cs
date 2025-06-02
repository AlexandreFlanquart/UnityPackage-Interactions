using UnityEngine;
using UnityEngine.Events;

public class SimpleAction : AInteractable
{
    [SerializeField] protected UnityEvent OnInteractionStarted = default;

    protected override void Init()
    {
        onInteract += StartSimpleAction;
    }

    public void StartSimpleAction()
    {
        if (gameObject.activeInHierarchy)
        {
            OnInteractionStarted?.Invoke();
            EndInteraction();
        }
    }
}
