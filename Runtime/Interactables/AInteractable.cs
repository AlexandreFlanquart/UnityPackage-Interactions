using System;
using System.Collections;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class AInteractable : MonoBehaviour
    {
        [SerializeField] private ActivationType activationType = ActivationType.OnStart;
        [SerializeField] private float delay = default;
        [SerializeField] private bool once = true;
        [SerializeField] private AInteractionType interactionType;
        [SerializeField] private AEffect[] effects;
        [SerializeField] private ACondition[] conditions;

        protected event Action onEnter;
        protected event Action onExit;
        protected event Action onInteract;

        private bool isActive = false;

        private enum ActivationType { OnStart, Manual }

        private bool isConditionsReady
        {
            get
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!conditions[i].CheckCondition()) return false;
                }
                return true;
            }
        }

        protected abstract void Init();

        protected virtual void Start()
        {
            Active(false);
            if (activationType == ActivationType.OnStart) StartCoroutine(WaitDelay());
            Init();
            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].onConditionMet += OnConditionChanged;
            }

            interactionType.onInteract += OnInteract;
            interactionType.onEnter += OnEnter;
            interactionType.onExit += OnExit;
        }

        public void OnConditionChanged()
        {
            if (interactionType.isWaitingCondition)
                OnInteract();
            Debug.Log("OnConditionChanged - isConditionsReady : " + isConditionsReady);
        }

        public void OnInteract()
        {
            Debug.Log("OnInteract Interactable" + isActive + ": " + isConditionsReady);
            if (!isActive || !isConditionsReady) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnInteract();
            }
            Active(false);
            onInteract?.Invoke();
        }

        public void OnEnter()
        {
            Debug.Log("OnEnter Interactable" + isActive + ": " + isConditionsReady);
            if (!isActive || !isConditionsReady) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnEnter();
            }
            onEnter?.Invoke();
        }

        public void OnExit()
        {
            Debug.Log("OnExit Interactable" + isActive + ": " + isConditionsReady);
            if (!isActive || !isConditionsReady) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnExit();
            }
            onExit?.Invoke();
        }

        protected void EndInteraction()
        {
            Debug.Log("EndInteraction : " + once);
            if (!once) Active(true);
        }

        public virtual void Active(bool pActive)
        {
            //Debug.Log("Active : " + pActive);
            isActive = pActive;
        }

        private IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(delay);
            Active(true);
        }
    }
}