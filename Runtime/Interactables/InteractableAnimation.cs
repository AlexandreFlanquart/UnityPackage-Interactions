using System.Collections;
using log4net.DateFormatter;
using MyUnityPackage.Toolkit;
using UnityEngine;
using TMPro;

namespace MyUnityPackage.Interactions
{
    public class InteractableAnimation : AInteractable
    {
        [SerializeField] private GameObject animatedObj;
        private bool isInteractionDone = false;

        private GameManager gameManager;
        private Animator animator;


        //[SerializeField] TextMeshProUGUI triggerText;
        [SerializeField] TextMeshProUGUI enableText;
        [SerializeField] TextMeshProUGUI requiredConditionsText;
        [SerializeField] TextMeshProUGUI allConditionsText;

        private string allConditionString;
        private string requiredConditionString;
        protected override void Start()
        {
            base.Start();
            gameManager = ServiceLocator.GetService<GameManager>();
            //triggerText.text = "Trigger => " + interactionTrigger.GetType().Name;
            allConditionString = allConditionsText.text;
            requiredConditionString = requiredConditionsText.text;
            UpdateTextConditions();
        }

        protected override void Init()
        {
            Debug.Log("Init");
            animator = animatedObj.GetComponent<Animator>();

            onEnterAction += OnStartEnter;
            onExitAction += OnStartExit;
            onInteractAction += OnStartInteract;
            onEnableAction += OnStartEnable;
            onDisableAction += OnStartDisable;
        }

        private void OnStartEnable()
        {
            animator.SetTrigger("Wait");
            enableText.text = "isEnabled => true";
            UpdateTextConditions();
        }

        private void OnStartDisable()
        {
            animator.SetTrigger("Disable");
            enableText.text = "isEnabled => false";
            UpdateTextConditions();
        }

        private void OnStartEnter()
        {
            Debug.Log("OnStartEnter : Rotate");
            animator.SetTrigger("Rotate");
            UpdateTextConditions();
        }

        private void OnStartExit()
        {
            Debug.Log("OnStartExit : Wait");
            animator.SetTrigger("Wait");
            UpdateTextConditions();
        }

        private void OnStartInteract()
        {
            gameManager.IncrementInteractionCount();
            animator.SetTrigger("Talk");

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationTime = stateInfo.normalizedTime * stateInfo.length;
            StartCoroutine(StopInteraction());
        }

        private IEnumerator StopInteraction()
        {
            yield return new WaitForSeconds(3);
            Debug.Log("StopInteraction");
            isInteractionDone = true;
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
