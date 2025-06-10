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

        [SerializeField] private CurrentState currentState = CurrentState.None;
        protected event Action onEnterAction;
        protected event Action onExitAction;
        protected event Action onInteractAction;

        private bool isActive = false;
        private bool hasRequiredForEnterConditions
        {
            get
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].requiredForEnter) return true;
                }
                return false;
            }
        }
        private enum ActivationType { OnStart, Manual }

        private enum CurrentState { None, TypeAndConditions, WaitingConditionsActive, WaitingTypesActive };
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
        private bool isRequiredForEnterConditionReady
        {
            get
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].requiredForEnter && !conditions[i].CheckCondition()) return false;
                }
                return true;
            }
        }
        bool lastStateIsExit = false;
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
        //Allow to trigger the function when a state has change
        public void OnConditionChanged(bool isInteracting)
        {
            if (isInteracting && CurrentState.WaitingTypesActive == currentState && isConditionsReady)
                OnInteract();
            else if (isInteracting)
                OnEnter();
            else if (!isInteracting)
            {
                Debug.Log("J'exit");
                //currentState = CurrentState.WaitingConditionsActive;
                OnExit();
            }

            Debug.Log("OnConditionChanged - isConditionsReady : " + isConditionsReady);
        }
        //Function to call when the main condition is good
        public void OnInteract()
        {
            Debug.Log("OnInteract Interactable" + isActive + ": " + isConditionsReady);
            if (!isActive || !isConditionsReady) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnInteract();
            }
            Active(false);
            currentState = CurrentState.TypeAndConditions;
            onInteractAction?.Invoke();
            lastStateIsExit = false;
        }
        //Function to call when the main condition can be call
        public void OnEnter()
        {
            if (hasRequiredForEnterConditions)
            {
                Debug.Log("OnEnter Interactable ForEnter" + isActive + ": " + isRequiredForEnterConditionReady);
                if (!isActive || !isRequiredForEnterConditionReady)
                    return;
            }
            else
            {
                Debug.Log("OnEnter Interactable" + isActive + ": " + isConditionsReady);
                if (!isActive || !isConditionsReady)
                    return;
            }

            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnEnter();
            }
            currentState = CurrentState.WaitingTypesActive;
            onEnterAction?.Invoke();
            lastStateIsExit = false;
        }
        //Function to call when the main condition can't be call anymore
        public void OnExit()
        {
            Debug.Log("OnExit Interactable" + isActive + ": " + isConditionsReady);

            if (!isActive || lastStateIsExit || currentState == CurrentState.None) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnExit();
            }
            if (isConditionsReady)
                currentState = CurrentState.WaitingTypesActive;
            else if (currentState == CurrentState.TypeAndConditions)
                currentState = CurrentState.WaitingConditionsActive;
            else
                currentState = CurrentState.None;

            onExitAction?.Invoke();
            lastStateIsExit = true;
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