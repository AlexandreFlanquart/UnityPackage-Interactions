using System;
using System.Collections;
using UnityEngine;

public abstract class AInteractable : MonoBehaviour
{
    [SerializeField] private ActivationType activationType = ActivationType.OnStart;
    [SerializeField] private float delay = default;
    [SerializeField] private bool once = true;
    [SerializeField] private AInteractionType interactionType;
    [SerializeField] private AEffect[] effects;
    [SerializeField] private ACondition[] conditions;

    protected event Action onEnter;
    protected event Action onExit;
    protected event Action onInteract;
    
    private bool isActive = false;

    private enum ActivationType { OnStart, Manual }

    private bool CanInteract
    {
        get
        {
            for(int i = 0; i < conditions.Length; i++)
            {
                if(!conditions[i].CheckCondition()) return false;
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
        interactionType.onInteract += OnInteract;
        interactionType.onEnter += OnEnter;
        interactionType.onExit += OnExit;
    }

    public void OnInteract()
    {
        if(!isActive || !CanInteract) return;
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].OnInteract();
        }
        Active(false);
        onInteract?.Invoke();
    }

    public void OnEnter()
    {
        if(!isActive || !CanInteract) return;
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].OnEnter();
        }
        onEnter?.Invoke();
    }

    public void OnExit()
    {
        if(!isActive || !CanInteract) return;
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].OnExit();
        }
        onExit?.Invoke();
    }

    protected void EndInteraction()
    {
        if(!once) Active(true);
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