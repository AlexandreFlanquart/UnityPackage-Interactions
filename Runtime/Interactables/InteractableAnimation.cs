using MyUnityPackage.Toolkit;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class InteractableAnimation : AInteractable
    {
        private bool isInteractionDone = false;

        private GameManager gameManager;

        void Start()
        {
            gameManager = ServiceLocator.GetService<GameManager>();

        }
        protected override void Init()
        {
            /*
            onEnterAction +=;
            onExitAction +=;
            onInteractAction += OnInteract;
           */
        }

        private void OnInteract()
        {
            gameManager.IncrementInteractionCount();
        }


    }
}
