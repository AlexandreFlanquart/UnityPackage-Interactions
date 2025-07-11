using System.Collections;
using UnityEngine;
using TMPro;

namespace MyUnityPackage.Interactions
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
            yield return new WaitForSeconds(0.1f);
            Debug.Log("animation" + animation);
            animator.SetTrigger(animation);
            //animator.Update(0f);
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
        }

        public IEnumerator StopInteraction(float animationTime)
        {
            Debug.Log("StopInteractionAnimationtime " + animationTime);
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
