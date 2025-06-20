using System.Collections;
using log4net.DateFormatter;
using MyUnityPackage.Toolkit;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class InteractableAnimation : AInteractable
    {
        [SerializeField] private GameObject animatedObj;
        private bool isInteractionDone = false;

        private GameManager gameManager;
        private Animator animator;

        protected override void Start()
        {
            base.Start();
            gameManager = ServiceLocator.GetService<GameManager>();
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
        }

        private void OnStartDisable()
        {
            animator.SetTrigger("Disable");
        }

        private void OnStartEnter()
        {
            animator.SetTrigger("Rotate");
        }

        private void OnStartExit()
        {
            Debug.Log("OnStartExit");
            animator.SetTrigger("Wait");
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
            isInteractionDone = true;
            EndInteraction();
        }
    }
}
