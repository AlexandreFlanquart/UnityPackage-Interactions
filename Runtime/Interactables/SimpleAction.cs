using UnityEngine;
using UnityEngine.Events;

namespace MyUnityPackage.Interactions
{
    public class SimpleAction : AInteractable
    {
        [SerializeField] protected UnityEvent OnInteractionStarted = default;

        protected override void Init()
        {
            onInteractAction += StartSimpleAction;
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
}