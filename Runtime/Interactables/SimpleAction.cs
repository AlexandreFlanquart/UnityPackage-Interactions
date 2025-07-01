using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MyUnityPackage.Interactions
{
    public class SimpleAction : AInteractable
    {
        [SerializeField] protected UnityEvent OnInteractionStarted = default;
        [SerializeField] protected float actionDelay = 1f;
       
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
                StartCoroutine(WaitDelay(actionDelay));
            }
        }


        private IEnumerator WaitDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EndInteraction();
        }
    }
}