using UnityEngine;
using UnityEngine.Events;
using MyUnityPackage.Toolkit;
public class InteractableItemQuestion : AInteractable
{
    [SerializeField ] private InteractionIcon interactionIcon;
    [SerializeField] private QuestionDataSO questionDataSO;
    [SerializeField] private UnityEvent onGoodAnswer;
    [SerializeField] private UnityEvent onBadAnswer;

    protected override void Init()
    {
        onEnter += delegate { interactionIcon.canInteract = true; };
        onExit += delegate { interactionIcon.canInteract = false; };
        onInteract += AskQuestion;
    }

    public void AskQuestion()
    {
        Debug.Log("AskQuestion");
       // ServiceLocator.GetService<UIManager>().UI_Question.Setup(questionDataSO, OnGoodAnswer, OnBadAnswer);
       // ServiceLocator.GetService<UIManager>().UI_Question.Show();
    }

    public void OnGoodAnswer()
    {
        onGoodAnswer?.Invoke();
    }

    public void OnBadAnswer()
    {
        onBadAnswer?.Invoke();
        // kill player
       // ServiceLocator.GetService<Player>().healthHandler.TakeDamage(1, DeathReason.BadAnswer);
    }
}