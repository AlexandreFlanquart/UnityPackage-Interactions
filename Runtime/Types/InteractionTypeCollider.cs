using UnityEngine;
using System;
public class InteractionTypeCollider : AInteractionType
{
    public override event Action onEnter;
    public override event Action onExit;
    public override event Action onInteract;

    public bool IsColliding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Begin Collision");
            onEnter?.Invoke();
        }

    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("End Collision");
            onExit?.Invoke();
        }

    }
}
