using System.Collections;
using UnityEngine;
using TMPro;

namespace MyUnityPackage.Interactions.Samples
{
    public class InteractableAnimation : AInteractable
    {
        [SerializeField] private GameObject animatedObj;
      
        private GameManagerInteractions gameManager;
        private Animator animator;

        [SerializeField] TextMeshProUGUI enableText;
        [SerializeField] TextMeshProUGUI requiredConditionsText;
        [SerializeField] TextMeshProUGUI allConditionsText;

        private string allConditionString;
        private string requiredConditionString;

        //public float animationTime = 1f;
        protected override void Start()
        {
            base.Start();
            gameManager = GameManagerInteractions.GetInstance();
            //triggerText.text = "Trigger => " + interactionTrigger.GetType().Name;
            allConditionString = allConditionsText.text;
            requiredConditionString = requiredConditionsText.text;
            UpdateTextConditions();
        }

        protected override void Init()
        {
            if (animatedObj != null)
                animator = animatedObj.GetComponent<Animator>();
            else
                Debug.LogWarning("InteractableAnimation: animatedObj is null");

            onEnterAction += OnStartEnter;
            onExitAction += OnStartExit;
            onInteractAction += OnStartInteract;
            onEnableAction += OnStartEnable;
            onDisableAction += OnStartDisable;
        }

        private void OnStartEnable()
        {
            if (animator != null) animator.SetTrigger("Wait");
            if (enableText != null) enableText.text = "isEnabled => true";
            UpdateTextConditions();
        }

        private void OnStartDisable()
        {
            if (animator != null) animator.SetTrigger("Disable");

            if (enableText != null) enableText.text = "isEnabled => false";
            UpdateTextConditions();
        }

        private void OnStartEnter()
        {
            if (animator != null) animator.SetTrigger("Rotate");
            UpdateTextConditions();
        }

        private void OnStartExit()
        {
            if (animator != null) animator.SetTrigger("Wait");
            UpdateTextConditions();
        }

        private void OnStartInteract()
        {
            gameManager.IncrementInteractionCount();
            if (animator != null)
            {
                animator.SetTrigger("Talk");
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onEnterAction -= OnStartEnter;
            onExitAction -= OnStartExit;
            onInteractAction -= OnStartInteract;
            onEnableAction -= OnStartEnable;
            onDisableAction -= OnStartDisable;
        }

        public IEnumerator StopInteraction(float animationTime)
        {
            yield return new WaitForSeconds(animationTime);
            
            EndInteraction();
            UpdateTextConditions();
        }

        private void UpdateTextConditions()
        {
            if (isConditionsReady)
                allConditionsText.text = allConditionString + "=> true";
            else
                allConditionsText.text = allConditionString + "=> false";
            if (isRequiredConditionsActives)
                requiredConditionsText.text = requiredConditionString + "=> true";
            else
                requiredConditionsText.text = requiredConditionString + "=> false";
        }
    }
}