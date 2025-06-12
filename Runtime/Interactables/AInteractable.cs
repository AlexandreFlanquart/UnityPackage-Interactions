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

        private CurrentState currentState = CurrentState.None;
        protected event Action onEnterAction;
        protected event Action onExitAction;
        protected event Action onInteractAction;

        private bool isEnable = false;
        protected bool hasTrigger = false;

        private enum ActivationType { OnStart, Manual }

        private enum CurrentState { None, onEnterActive, onInteractActive };
        private bool isConditionsReady
        {
            get
            {
                bool isReady = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!conditions[i].CheckCondition()) isReady = false;
                }
                return isReady;
            }
        }
        private bool isRequiredConditionsActives
        {
            get
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].requiredForEffect && !conditions[i].CheckCondition()) return false;
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

            currentState = CurrentState.None;
            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].onConditionMet += OnConditionChanged;
            }

            if (interactionType != null)
            {
                hasTrigger = true;
                interactionType.onInteract += OnInteractTrigger;
                interactionType.onEnter += OnEnterTrigger;
                interactionType.onExit += OnExitTrigger;
            }
            else
            {
                hasTrigger = false;
            }

        }
        //Allow to trigger the function when a state has change
        public void OnConditionChanged(bool isInteracting)
        {
            Debug.Log("OnConditionChanged");

            if (isInteracting && currentState == CurrentState.None && isRequiredConditionsActives)
            {
                Debug.Log("OnEnter");
                OnEnter();
            }
            else if (!isInteracting && currentState == CurrentState.onEnterActive)
            {
                Debug.Log("exit");
                OnExit();
            }

            if (isInteracting && currentState == CurrentState.onEnterActive && isConditionsReady && !hasTrigger)
            {
                Debug.Log("OnInteract");
                OnInteract();
            }

            Debug.Log("isConditionsReady : " + isConditionsReady);
        }

        private void OnInteractTrigger()
        {
            Debug.Log("OnInteractType");
            if (isConditionsReady)
            {
                Debug.Log("OnInteract");
                OnInteract();
            }
        }

        protected virtual void OnEnterTrigger()
        {
            Debug.Log("OnEnter triger");
            // check condition pour afficher les manquantes

        }

        protected virtual void OnExitTrigger()
        {
            Debug.Log("OnExit trigger");

        }

        //Function to call when the main condition is good
        public void OnInteract()
        {
            Debug.Log("OnInteract Interactable" + isEnable);
            if (!isEnable) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnInteract();
            }
            Active(false);
            currentState = CurrentState.onInteractActive;
            onInteractAction?.Invoke();
        }

        //Function to call when the main condition can be call
        public void OnEnter()
        {
            if (!isEnable)
                return;

            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnEnter();
            }
            currentState = CurrentState.onEnterActive;
            onEnterAction?.Invoke();
        }

        //Function to call when the main condition can't be call anymore
        public void OnExit()
        {
            Debug.Log("OnExit Interactable" + isEnable);

            if (!isEnable) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnExit();
            }

            currentState = CurrentState.None;

            onExitAction?.Invoke();
        }

        protected void EndInteraction()
        {
            Debug.Log("EndInteraction : " + once);
            if (!once) Active(true);
            currentState = CurrentState.None;
            OnExit();
            if (true)
            {
                // to dos
            }
        }

        public virtual void Active(bool pActive)
        {
            //Debug.Log("Active : " + pActive);
            isEnable = pActive;
        }

        private IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(delay);
            Active(true);
        }
    }
}