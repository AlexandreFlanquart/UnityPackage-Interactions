using System.Collections;
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
                Debug.Log("StartSimpleAction");
                OnInteractionStarted?.Invoke();
                StartCoroutine(WaitDelay(3));
            }
        }


        private IEnumerator WaitDelay(int delay)
        {
            yield return new WaitForSeconds(delay);
            EndInteraction();
        }
    }
}