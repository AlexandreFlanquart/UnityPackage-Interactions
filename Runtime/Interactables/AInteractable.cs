using System;
using System.Collections;
using PlasticGui.Help.Conditions;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class AInteractable : MonoBehaviour
    {
        [SerializeField] private ActivationType activationType = ActivationType.OnStart;
        [SerializeField] private float delay = default;
        [SerializeField] private bool once = true;

        [SerializeField] protected AInteractionTrigger interactionTrigger;
        [SerializeField] private AEffect[] effects;
        [SerializeField] private ACondition[] conditions;

        private CurrentState currentState = CurrentState.None;
        protected event Action onEnableAction;
        protected event Action onDisableAction;
        protected event Action onEnterAction;
        protected event Action onExitAction;
        protected event Action onInteractAction;

        protected bool isEnable = false;
        protected bool hasTrigger = false;
        protected bool hasConditions = false;
        private bool isInteractionRunning = false;

        private enum ActivationType { OnStart, Manual }

        private enum CurrentState { None, onEnterActive, onInteractActive };

        protected bool isConditionsReady;
        protected bool IsConditionsReady
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
        protected bool isRequiredConditionsActives;
        protected bool IsRequiredConditionsActives
        {
            get
            {
                bool hasRequiredCondition = false;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].requiredForEffects)
                    {
                        if (!conditions[i].CheckCondition())
                            return false;
                        hasRequiredCondition = true;
                    }
                }
                if (!hasRequiredCondition) return isConditionsReady;
                return hasRequiredCondition;
            }
        }
        protected abstract void Init();

        protected virtual void Start()
        {
            Disable();
            if (activationType == ActivationType.OnStart) StartCoroutine(WaitDelay());
            Init();

            currentState = CurrentState.None;
            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].onConditionMet += OnConditionChanged;
            }

            if (interactionTrigger != null)
            {
                hasTrigger = true;
                interactionTrigger.onInteract += OnInteractTrigger;
                interactionTrigger.onEnter += OnEnterTrigger;
                interactionTrigger.onExit += OnExitTrigger;
            }
            else
            {
                hasTrigger = false;
            }

            if (conditions.Length > 0)
                hasConditions = true;

            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;
        }

        //Allow to trigger the function when a state has change
        public void OnConditionChanged(bool isInteracting)
        {
            isConditionsReady = IsConditionsReady;
            isRequiredConditionsActives = IsRequiredConditionsActives;

            Debug.Log("OnConditionChanged : isInteracting : " + isInteracting + " currentState " + currentState + " isRequiredConditionsActives " + isRequiredConditionsActives);

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
            Debug.Log("isRequiredConditionsActives : " + isRequiredConditionsActives);
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
            Debug.Log("OnEnter trigger");
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
            if (!isEnable || isInteractionRunning) return;
            Debug.Log("OnInteract isInteractionRunning" + isInteractionRunning);
            isInteractionRunning = true;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnInteract();
            }

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
        protected IEnumerator EndInteractionCoroutine()
        {
            Debug.Log("EndInteraction : " + once);
            currentState = CurrentState.None;
            OnExit();
            yield return null;
            if (once)
            {
                Disable();
            }
            else
            {
                if (isConditionsReady && !hasTrigger)
                {
                    Debug.Log("OnInteract");
                    OnInteract();
                }
                else if (isRequiredConditionsActives && hasConditions)
                {
                    Debug.Log("OnEnter");
                    OnEnter();
                }
            }
            isInteractionRunning = false;
        }
        protected void EndInteraction()
        {
            StartCoroutine(EndInteractionCoroutine());
        }

        public virtual void Enable()
        {
            //Debug.Log("Active : " + pActive);
            isEnable = true;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnEnable();
            }
            onEnableAction?.Invoke();
        }

        public virtual void Disable()
        {
            isEnable = false;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].OnDisable();
            }
            onDisableAction?.Invoke();
        }

        private IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(delay);
            Enable();
        }
    }
}