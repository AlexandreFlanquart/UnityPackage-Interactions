# Creating Effects

Effects are visual, audio, or logic responses during the lifecycle of an interaction.

## Lifecycle Methods

```csharp
public class MyEffect : AEffect
{
    public override void ActivateEffect() { }      // Interaction available
    public override void DeactivateEffect() { }    // Interaction unavailable
    public override void EnterEffect() { }         // All conditions required ✓
    public override void ExitEffect() { }          // Condition fails
    public override void InteractEffect() { }      // Player interacts + ALL conditions ✓
}
```


## Example: Sound Effect

```csharp
public class SoundEffect : AEffect
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip activationSound;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private AudioClip exitSound;
    [SerializeField] private AudioClip deactivationSound;
    
    public override void ActivateEffect()
    {
        if (activationSound != null)
            audioSource.PlayOneShot(activationSound);
    }
    
    public override void EnterEffect()
    {
        if (enterSound != null)
            audioSource.PlayOneShot(enterSound);
    }
    
    public override void InteractEffect()
    {
        if (interactionSound != null)
            audioSource.PlayOneShot(interactionSound);
    }
    
    public override void ExitEffect()
    {
        if (exitSound != null)
            audioSource.PlayOneShot(exitSound);
    }
    
    public override void DeactivateEffect()
    {
        if (deactivationSound != null)
            audioSource.PlayOneShot(deactivationSound);
    }
}
```

## Example: Animation Effect

```csharp
public class AnimationEffect : AEffect
{
    [SerializeField] private Animator animator;
    
    public override void EnterEffect() 
    { 
        if (animator != null) animator.SetTrigger("Enter");
    }
    
    public override void InteractEffect() 
    { 
        if (animator != null) animator.SetTrigger("Interact");
    }
    
    public override void ExitEffect() 
    { 
        if (animator != null) animator.SetTrigger("Exit");
    }
    
    public override void ActivateEffect() 
    { 
        if (animator != null) animator.SetBool("Active", true);
    }
    
    public override void DeactivateEffect() 
    { 
        if (animator != null) animator.SetBool("Active", false);
    }
}
```

## Best Practices

✅ Check for null references before using them  
✅ Avoid allocations (use object pools if necessary)  
✅ Create reusable and parameterized effects  
✅ Leave lists empty by default (implement only what is necessary)
