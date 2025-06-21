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

        [SerializeField] TextMeshProUGUI enableText;
        [SerializeField] TextMeshProUGUI requiredConditionsText;
        [SerializeField] TextMeshProUGUI allConditionsText;

        protected override void Start()
        {
            base.Start();
            gameManager = ServiceLocator.GetService<GameManager>();
            //triggerText.text = "Trigger => " + interactionTrigger.GetType().Name;
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
            Debug.Log("animation enable wait");
            animator.SetTrigger("Wait");
            enableText.text = "isEnabled => true";
            UpdateTextConditions();
        }

        private void OnStartDisable()
        {
            Debug.Log("animation Disable");

            StartCoroutine(AnimationDelay("Disable"));

            enableText.text = "isEnabled => false";
            UpdateTextConditions();
        }

        private void OnStartEnter()
        {
            Debug.Log("animation Rotate");

            StartCoroutine(AnimationDelay("Rotate"));
            UpdateTextConditions();
        }

        private IEnumerator AnimationDelay(string animation)
        {
            yield return null;
            animator.SetTrigger(animation);
        }

        private void OnStartExit()
        {
            Debug.Log("animation exit wait");

            Debug.Log("OnStartExit");
            StartCoroutine(AnimationDelay("Wait"));
            UpdateTextConditions();
        }

        private void OnStartInteract()
        {
            Debug.Log("animation Talk");

            gameManager.IncrementInteractionCount();
            StartCoroutine(AnimationDelay("Talk"));

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationTime = stateInfo.normalizedTime * stateInfo.length;
            StartCoroutine(StopInteraction());
        }

        private IEnumerator StopInteraction()
        {
            yield return new WaitForSeconds(3);
            isInteractionDone = true;
            EndInteraction();
            UpdateTextConditions();
        }

        private void UpdateTextConditions()
        {
            if (isConditionsReady)
                allConditionsText.text = "AllConditionsReady => true";
            else
                allConditionsText.text = "AllConditionsReady => false";
            if (isRequiredConditionsActives)
                requiredConditionsText.text = "RequiredConditionsReady => true";
            else
                requiredConditionsText.text = "RequiredConditionsReady => false";
        }
    }
}
